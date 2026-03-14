using Widget.Api.ApiModels;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public class DeadlineHeroTarget : ITarget
{
    public Achievement Achievement => Achievement.DeadlineHero;

    public AchievementResult Achieve(PostEventApiModel action, IReadOnlyCollection<TasksRepository.TaskItem> tasks)
    {
        if (action.Event != EventType.ISSUE_RESOLVED || (action.DueDate.HasValue && DateTime.UtcNow < DateTimeOffset.FromUnixTimeMilliseconds(action.DueDate.Value)))
            return new AchievementResult(false, 0);
        
        return new AchievementResult(true, 100);
    }
}