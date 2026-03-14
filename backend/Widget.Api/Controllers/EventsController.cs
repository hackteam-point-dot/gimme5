using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using Widget.Api.ApiModels;
using Widget.Api.Application;
using Widget.Api.Repositories;

namespace Widget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(
    TasksRepository tasksRepository, 
    AchievementService achievementService,
    XpService xpService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<EventApiResponse?>> PostEvent([FromBody] PostEventApiModel args)
    {
        var taskType = args.Event switch
        {
            EventType.ISSUE_RESOLVED => TasksRepository.TaskType.Issue,
            EventType.BUG_RESOLVED => TasksRepository.TaskType.Bug,
            EventType.BUG_CREATED => TasksRepository.TaskType.Bug
        };
        
        var task = await tasksRepository.CreateOrUpdateAsync(new TasksRepository.TaskItem(args.IssueId, args.ProjectKey,
            args.Event, taskType, args.Login, false, DateTime.UtcNow, args.Children?.ToImmutableList() ?? []));
        
        if (task?.ExpAwarded == true)
            return Ok(null);
        
        var achievementResult = await achievementService.CalculateAchievements(args, args.Login);
        var actualExp = await xpService.TryAddXp(args, achievementResult.Exp);

        if (actualExp.ExpChange == 0)
            return Ok(new EventApiResponse(actualExp.Exp, actualExp.ExpChange, actualExp.LevelUpgradedTo,
                achievementResult.AchievementName, achievementResult.Exp));
        
        await tasksRepository.SetExpAwardedAsync(args.IssueId, true);

        return Ok(new EventApiResponse(actualExp.Exp, actualExp.ExpChange, actualExp.LevelUpgradedTo,
            achievementResult.AchievementName, achievementResult.Exp));
    }
}