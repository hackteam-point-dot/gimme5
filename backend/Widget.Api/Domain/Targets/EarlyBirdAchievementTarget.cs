using Widget.Api.ApiModels;
using Widget.Api.Application;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public class NightOwlTarget : ITarget
{
    public Achievement Achievement => Achievement.NightOwl;

    public AchievementResult Achieve(PostEventApiModel action, ProjectConfiguration? config,
        IReadOnlyCollection<TasksRepository.TaskItem> tasks)
    {
        var isEnabled = config == null || !config.AchievementEnabled.ContainsKey(Achievement) ||
                        config.AchievementEnabled[Achievement];
        
        if (!isEnabled)
            return AchievementResult.NoResult;
        
        var hour = DateTime.UtcNow.Hour;

        if ((action.Event == EventType.ISSUE_RESOLVED ||
             action.Event == EventType.BUG_RESOLVED) &&
            (hour >= 19 || hour < 1))
        {
            var reward = config?.AchievementRewards.GetValueOrDefault(Achievement) ?? 20;
            
            return new AchievementResult(true, (ulong)reward);
        }

        return AchievementResult.NoResult;
    }
}
