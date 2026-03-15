using Widget.Api.ApiModels;
using Widget.Api.Application;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public interface ITarget
{
    AchievementResult IsAchieved(PostEventApiModel action, ProjectConfiguration? config, IReadOnlyCollection<TasksRepository.TaskItem> tasks);
    Achievement Achievement { get; }
}

public interface ISecretTarget
{
    AchievementResult IsAchieved(string userId, int score);
    Achievement Achievement { get; }
}