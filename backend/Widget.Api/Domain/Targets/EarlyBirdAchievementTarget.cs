using Widget.Api.ApiModels;
using Widget.Api.Domain;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public class EarlyBirdTarget : ITarget
{
    public Achievement Achievement => Achievement.EarlyBird;

    public AchievementResult Achieve(PostEventApiModel action, IReadOnlyCollection<TasksRepository.TaskItem> tasks)
    {
        if ((action.Event == EventType.ISSUE_RESOLVED ||
            action.Event == EventType.BUG_RESOLVED) && 
            action.DueDate.HasValue && DateTime.UtcNow < DateTimeOffset.FromUnixTimeMilliseconds(action.DueDate.Value))
            return new AchievementResult(true, 20);
        
        return new AchievementResult(false, 0);
    }
}
