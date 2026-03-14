using Widget.Api.ApiModels;
using Widget.Api.Domain;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public class TaskBuilderTarget : ITarget
{
    public Achievement Achievement => Achievement.TaskBuilder;

    public AchievementResult Achieve(PostEventApiModel action, IReadOnlyCollection<TasksRepository.TaskItem> tasks)
    {
        return new AchievementResult(true, 50);
    }
}