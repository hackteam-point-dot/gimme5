using Microsoft.AspNetCore.Mvc;
using Widget.Api.ApiModels;
using Widget.Api.Application;
using Widget.Api.Repositories;

namespace Widget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(
    UserRepository userRepository,
    TasksRepository tasksRepository,
    UserAchievementRepository userAchievementRepository,
    XpService xpService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PostEvent([FromBody] PostEventApiModel args, [FromQuery] string projectId)
    {
        switch (args.Event)
        {
            case EventType.ISSUE_RESOLVED:
                // TODO: stub logic for ISSUE_RESOLVED
                break;

            case EventType.STORY_DONE:
                await xpService.TryAddXp(args);

                break;

            case EventType.BUG_CREATED:
                // TODO: stub logic for BUG_CREATED
                break;

            case EventType.BUG_RESOLVED:
                // TODO: stub logic for BUG_RESOLVED
                break;

            case EventType.SPRINT_STARTED:
                // TODO: stub logic for SPRINT_STARTED
                break;

            case EventType.SPRINT_FINISHED:
                // TODO: stub logic for SPRINT_FINISHED
                break;

            default:
                return BadRequest($"Unsupported event type: {args.Event}");
        }

        return Ok(args);
    }
}