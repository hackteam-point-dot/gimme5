using Widget.Api.ApiModels;
using Widget.Api.Application;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public class BugHunterTarget : ITarget
{
    public Achievement Achievement => Achievement.BugHunter;

    public AchievementResult Achieve(PostEventApiModel action, ProjectConfiguration? config,
        IReadOnlyCollection<TasksRepository.TaskItem> tasks)
    {
        var isEnabled = config == null || !config.AchievementEnabled.ContainsKey(Achievement) ||
                        config.AchievementEnabled[Achievement];
        
        if (!isEnabled)
            return AchievementResult.NoResult;

        if (action.Event != EventType.BUG_RESOLVED)
            return AchievementResult.NoResult;

        var reward = config?.AchievementRewards.GetValueOrDefault(Achievement) ?? 150;
        var achievementTasks = tasks.Where(x => x.Type == TasksRepository.TaskType.Bug).ToList();
        var bugsCount = achievementTasks.Count();
        var achieved = bugsCount % 5 == 0;

        return new AchievementResult(achieved, (ulong)reward, achievementTasks.Select(x => x.Id).ToArray());
    }
}
