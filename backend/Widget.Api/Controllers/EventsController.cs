using Microsoft.AspNetCore.Mvc;
using Widget.Api.ApiModels;
using Widget.Api.Repositories;

namespace Widget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(UserRepository userRepository, TasksRepository tasksRepository, UserAchievementRepository userAchievementRepository) : ControllerBase
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
                if (!string.IsNullOrEmpty(args.IssueId))
                {
                    var subtasks = await tasksRepository.GetSubtasksAsync(args.IssueId);
                    foreach (var subtask in subtasks)
                    {
                        if (subtask.StoryPoints > 0 && !string.IsNullOrEmpty(subtask.AssigneeId))
                        {
                            var xp = (ulong)subtask.StoryPoints * 10;
                            await userAchievementRepository.IncrementXpAsync(subtask.AssigneeId, xp);
                        }
                    }
                }

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
