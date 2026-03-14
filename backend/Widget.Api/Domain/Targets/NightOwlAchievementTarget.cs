using Widget.Api.Domain;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public class NightOwlTarget : ITarget
{
    public Achievement Achievement => Achievement.NightOwl;

    public bool IsAchieved(string userId, IReadOnlyCollection<TasksRepository.TaskItem> tasks)
    {
        return true;
    }
}
