using Widget.Api.ApiModels;
using Widget.Api.Repositories;

namespace Widget.Api.Application;

public class AchievementService
{
    public Task TryAddAchievement(TasksRepository.TaskItem item)
    {
        return Task.CompletedTask;
    }
}