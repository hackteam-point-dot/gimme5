using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public class DeadlineHeroTarget : ITarget
{
    public Achievement Achievement => Achievement.DeadlineHero;

    public bool IsAchieved(string userId, IReadOnlyCollection<TasksRepository.TaskItem> tasks)
    {
        return true;
    }
}