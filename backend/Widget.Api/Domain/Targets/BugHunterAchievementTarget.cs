using Widget.Api.Domain;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public class BugHunterTarget : ITarget
{
    public Achievement Achievement => Achievement.BugHunter;

    public bool IsAchieved(string userId, IReadOnlyCollection<TasksRepository.TaskItem> tasks)
    {
        return true;
    }
}
