using Widget.Api.Domain;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public class OnFireTarget : ITarget
{
    public Achievement Achievement => Achievement.OnFire;

    public bool IsAchieved(string userId, IReadOnlyCollection<TasksRepository.TaskItem> tasks)
    {
        return true;
    }
}
