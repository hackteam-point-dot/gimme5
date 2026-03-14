using Widget.Api.ApiModels;
using Widget.Api.Domain.Targets;
using Widget.Api.Repositories;

namespace Widget.Api.Application;

public class AchievementService(
    TasksRepository tasksRepository,
    IEnumerable<ITarget> achievements)
{
    public async Task CalculateAchievements(string userId)
    {
        var tasks = await tasksRepository.GetAllAsync();

        foreach (var a in achievements)
        {
            var isAchieved = a.IsAchieved(userId, tasks);
        }
    }
}