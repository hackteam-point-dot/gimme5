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
    public async Task<ActionResult<PostEventApiModel>> PostEvent([FromBody] PostEventApiModel args, [FromQuery] string projectId)
    {
        var task = await tasksRepository.CreateOrUpdateAsync(new TasksRepository.TaskItem(args.IssueId, args.ProjectKey,
            EventType.ISSUE_RESOLVED, args.Login, args.Children?.ToImmutableList() ?? []));
        
        var actualExp = await xpService.TryAddXp(args);
        await achievementService.TryAddAchievement(args);
        
        return Ok(new EventApiResponse(actualExp.Exp, actualExp.ExpChange));
    }
}