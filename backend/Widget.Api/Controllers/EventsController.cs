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
    ExpService expService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<EventApiResponse?>> PostEvent([FromBody] PostEventApiModel args)
    {
        var taskType = args.Event switch
        {
            EventType.ISSUE_RESOLVED => TasksRepository.TaskType.Issue,
            EventType.BUG_RESOLVED => TasksRepository.TaskType.Bug,
            EventType.BUG_CREATED => TasksRepository.TaskType.Bug,
            EventType.BUG_IN_PROGRESS => TasksRepository.TaskType.Bug,
            EventType.ISSUE_IN_PROGRESS => TasksRepository.TaskType.Issue
        };
        
        var task = await tasksRepository.CreateOrUpdateAsync(new TasksRepository.TaskItem(args.IssueId, args.ProjectKey,
            args.Event, taskType, args.Login, false, DateTime.UtcNow, args.Children?.ToImmutableList() ?? []));
        
        if (task?.ExpAwarded == true)
            return Ok(new EventApiResponse(0, 0, null, null, 0, null));
        
        var achievementResult = await achievementService.CalculateAchievements(task!, args, args.Login);
        var actualExp = await expService.TryAddExp(args, achievementResult);

        if (achievementResult.Exp != 0 || actualExp.Exp != 0)
            await tasksRepository.SetExpAwardedAsync(args.IssueId, true);

        return Ok(new EventApiResponse(actualExp.Exp, actualExp.ExpChange, actualExp.LevelUpgradedTo,
            achievementResult.AchievementName, achievementResult.Exp, actualExp.HeroClass));
    }

    [HttpPost]
    [Route("flappy-bug")]
    public async Task FlappyScore([FromBody]FlappyScoreApiRequest score, CancellationToken ct)
    {
        
    }
}