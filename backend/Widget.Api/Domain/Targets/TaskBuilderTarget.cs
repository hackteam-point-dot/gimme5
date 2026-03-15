using Widget.Api.ApiModels;
using Widget.Api.Application;
using Widget.Api.Domain;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public class TaskBuilderTarget : ITarget
{
    public Achievement Achievement => Achievement.TaskBuilder;

    public AchievementResult Achieve(PostEventApiModel action, ProjectConfiguration? config,
        IReadOnlyCollection<TasksRepository.TaskItem> tasks)
    {
        var isEnabled = config == null || !config.AchievementEnabled.ContainsKey(Achievement) ||
                        config.AchievementEnabled[Achievement];

        if (!isEnabled)
            return AchievementResult.NoResult;

        var reward = config?.AchievementRewards.GetValueOrDefault(Achievement) ?? 50;
        
        return new AchievementResult(true, (ulong)reward);
    }
}