using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public interface ITarget
{
    bool IsAchieved(string userId, IReadOnlyCollection<TasksRepository.TaskItem> tasks);
}