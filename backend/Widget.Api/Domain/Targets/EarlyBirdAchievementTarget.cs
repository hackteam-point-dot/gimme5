using Widget.Api.Domain;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public class EarlyBirdTarget : ITarget
{
    public Achievement Achievement => Achievement.EarlyBird;

    public bool IsAchieved(string userId, IReadOnlyCollection<TasksRepository.TaskItem> tasks)
    {
        return true;
    }
}
