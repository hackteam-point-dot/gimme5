using Widget.Api.ApiModels;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public class NightOwlTarget : ITarget
{
    public Achievement Achievement => Achievement.NightOwl;

    public AchievementResult Achieve(PostEventApiModel action, IReadOnlyCollection<TasksRepository.TaskItem> tasks)
    {
        var hour = DateTime.UtcNow.Hour;

        if ((action.Event == EventType.ISSUE_RESOLVED ||
             action.Event == EventType.BUG_RESOLVED) &&
            (hour >= 19 || hour < 1))
            return new AchievementResult(true, 20);

        return new AchievementResult(false, 0);
    }
}
