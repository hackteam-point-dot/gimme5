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
        if (args.Event == EventType.STORY_DONE && !string.IsNullOrEmpty(args.TaskId))
        {
            var subtasks = await tasksRepository.GetSubtasksAsync(args.TaskId);
            foreach (var subtask in subtasks)
            {
                if (subtask.StoryPoints > 0 && !string.IsNullOrEmpty(subtask.AssigneeId))
                {
                    var xp = (ulong)subtask.StoryPoints * 10;
                    await userAchievementRepository.IncrementXpAsync(subtask.AssigneeId, xp);
                }
            }
        }
        
        return Ok(args);
    }
}
