using Widget.Api.ApiModels;
using Widget.Api.Domain;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public class OnFireTarget : ITarget
{
    public Achievement Achievement => Achievement.OnFire;

    public AchievementResult Achieve(PostEventApiModel action, IReadOnlyCollection<TasksRepository.TaskItem> tasks)
    {
        var achieved = tasks
            .Where(t => t.ResolverId == action.Login && t.DateResolved.HasValue)
            .GroupBy(t => t.DateResolved!.Value.Date)
            .Any(g => g.Count() >= 5);

        return new AchievementResult(achieved, achieved ? 200 : 0UL);
    }
}
