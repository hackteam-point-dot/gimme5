using System.Text;
using Widget.Api.ApiModels;
using Widget.Api.Domain;
using Widget.Api.Domain.Targets;
using Widget.Api.Repositories;

namespace Widget.Api.Application;

public class AchievementService(
    TasksRepository tasksRepository,
    IEnumerable<ITarget> systemAchievements,
    UserAchievementRepository userAchievementRepository,
    ProjectConfigurationRepository projectConfigurationRepository)
{
    public record AchievementResult(string AchievementName, ulong Exp, string[] TaskIds, Achievement[] Achievements);

    public async Task<AchievementResult> CalculateAchievements(TasksRepository.TaskItem task, PostEventApiModel action,
        string userId)
    {
        if (task.ExpAwarded)
            return new AchievementResult(string.Empty, 0, [], []);

        var tasks = await tasksRepository.GetAllAsync();
        var config = await projectConfigurationRepository.GetByProjectIdAsync(action.ProjectKey);

        var awardedAchievements = new List<Achievement>();
        ulong totalExp = 0;
        var taskIds = new List<string>();

        foreach (var a in systemAchievements)
        {
            var result = a.Achieve(action, config, tasks);

            if (result.IsAchieved)
            {
                await userAchievementRepository.CreateOrUpdateAsync(userId, a.Achievement);
                awardedAchievements.Add(a.Achievement);
                totalExp += result.Exp;
                taskIds.AddRange(result.TaskIds);
            }
        }

        return new AchievementResult(string.Join(", ", awardedAchievements), totalExp, taskIds.Distinct().ToArray(),
            awardedAchievements.ToArray());
    }
}