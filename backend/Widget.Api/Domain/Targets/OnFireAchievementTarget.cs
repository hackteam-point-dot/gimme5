using Widget.Api.ApiModels;
using Widget.Api.Application;
using Widget.Api.Domain;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public class OnFireTarget : ITarget
{
    public Achievement Achievement => Achievement.OnFire;

    public AchievementResult Achieve(PostEventApiModel action, ProjectConfiguration? config,
        IReadOnlyCollection<TasksRepository.TaskItem> tasks)
    {
        var isEnabled = config == null || !config.AchievementEnabled.ContainsKey(Achievement) ||
                        config.AchievementEnabled[Achievement];
        
        if (!isEnabled || (action.Event != EventType.ISSUE_RESOLVED && action.Event != EventType.BUG_RESOLVED))
            return AchievementResult.NoResult;
        
        var reward = config?.AchievementRewards.GetValueOrDefault(Achievement) ?? 200;
        var achievementTasks = tasks
            .Where(t => t.ResolverId == action.Login && t.DateResolved.HasValue)
            .GroupBy(t => t.DateResolved!.Value.Date)
            .Where(g => g.Count() >= 5)
            .ToList();

        var achieved = achievementTasks.Any();

        return new AchievementResult(achieved, achieved ? (ulong) reward : 0UL,
            achievementTasks.SelectMany(x => x).Select(x => x.Id).Distinct().ToArray());
    }
}
