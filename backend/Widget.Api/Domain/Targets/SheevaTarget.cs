using Widget.Api.ApiModels;
using Widget.Api.Application;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public class SheevaTarget : ITarget
{
    public Achievement Achievement => Achievement.Sheeva;

    public AchievementResult Achieve(PostEventApiModel action, ProjectConfiguration? config,
        IReadOnlyCollection<TasksRepository.TaskItem> tasks)
    {
        var isEnabled = config == null || !config.AchievementEnabled.ContainsKey(Achievement) ||
                        config.AchievementEnabled[Achievement];
        
        if (!isEnabled)
            return AchievementResult.NoResult;
        
        if (action.Event != EventType.BUG_IN_PROGRESS && action.Event != EventType.ISSUE_IN_PROGRESS)
            return AchievementResult.NoResult;

        if (tasks.Count(x => x.Status == EventType.BUG_IN_PROGRESS || x.Status == EventType.ISSUE_IN_PROGRESS) >= 3)
        {
            var reward = config?.AchievementRewards.GetValueOrDefault(Achievement) ?? 180;
            return new AchievementResult(true, (ulong)reward);
        }
        
        return AchievementResult.NoResult;
    }
}