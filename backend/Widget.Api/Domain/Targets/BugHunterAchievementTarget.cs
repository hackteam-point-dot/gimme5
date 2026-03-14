using Widget.Api.ApiModels;
using Widget.Api.Domain;
using Widget.Api.Repositories;

namespace Widget.Api.Domain.Targets;

public class BugHunterTarget : ITarget
{
    public Achievement Achievement => Achievement.BugHunter;

    public AchievementResult Achieve(PostEventApiModel action, IReadOnlyCollection<TasksRepository.TaskItem> tasks)
    {
        if (action.Event != EventType.BUG_RESOLVED)
            return new AchievementResult(false, 0);
        
        var bugsCount = tasks.Count(x => x.Type == TasksRepository.TaskType.Bug);
        var achieved = bugsCount % 5 == 0;
        
        return new AchievementResult(achieved, 150);
    }
}
