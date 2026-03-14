using Widget.Api.ApiModels;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public interface ITarget
{
    AchievementResult Achieve(PostEventApiModel action, IReadOnlyCollection<TasksRepository.TaskItem> tasks);
    Achievement Achievement { get; }
}