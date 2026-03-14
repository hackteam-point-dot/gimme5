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
    public async Task<ActionResult<PostEventApiModel?>> PostEvent([FromBody] PostEventApiModel args)
    {
        var task = await tasksRepository.CreateOrUpdateAsync(new TasksRepository.TaskItem(args.IssueId, args.ProjectKey,
            EventType.ISSUE_RESOLVED, args.Login, false, args.Children?.ToImmutableList() ?? []));
        
        if (task?.ExpAwarded == true)
            return Ok(null);
        
        var actualExp = await xpService.TryAddXp(args);
        
        if (task != null)
            await achievementService.CalculateAchievements(args.Login);

        if (actualExp.ExpChange == 0) 
            return Ok(new EventApiResponse(actualExp.Exp, actualExp.ExpChange, actualExp.LevelUpgradedTo));
        
        await tasksRepository.SetExpAwardedAsync(args.IssueId, true);
            
        return Ok(new EventApiResponse(actualExp.Exp, actualExp.ExpChange, actualExp.LevelUpgradedTo));
    }
}