using Widget.Api.ApiModels;
using Widget.Api.Application;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public class DeadlineHeroTarget : ITarget
{
    public Achievement Achievement => Achievement.DeadlineHero;

    public AchievementResult Achieve(PostEventApiModel action, ProjectConfiguration? config,
        IReadOnlyCollection<TasksRepository.TaskItem> tasks)
    {
        var isEnabled = config == null || !config.AchievementEnabled.ContainsKey(Achievement) ||
                        config.AchievementEnabled[Achievement];
        
        if (!isEnabled)
            return AchievementResult.NoResult;

        if (action.Event != EventType.ISSUE_RESOLVED)
            return AchievementResult.NoResult;

        if (action.DueDate.HasValue &&
            DateTime.UtcNow <
            DateTimeOffset.FromUnixTimeMilliseconds(action.DueDate.Value))
        {
            var reward = config?.AchievementRewards.GetValueOrDefault(Achievement) ?? 100;
        
            return new AchievementResult(true, (ulong)reward, [action.IssueId]);
        }
        
        return AchievementResult.NoResult;
    }
}