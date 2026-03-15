using System.Text;
using Widget.Api.ApiModels;
using Widget.Api.Domain.Targets;
using Widget.Api.Repositories;

namespace Widget.Api.Application;

public class AchievementService(
    TasksRepository tasksRepository,
    IEnumerable<ITarget> systemAchievements,
    UserAchievementRepository userAchievementRepository,
    ProjectConfigurationRepository projectConfigurationRepository)
{
    public record AchievementResult(string AchievementName, ulong Exp);

    public async Task<AchievementResult> CalculateAchievements(TasksRepository.TaskItem task, PostEventApiModel action,
        string userId)
    {
        if (task.ExpAwarded)
            return new AchievementResult(string.Empty, 0);
        
        var tasks = await tasksRepository.GetAllAsync();
        var config = await projectConfigurationRepository.GetByProjectIdAsync(action.ProjectKey);

        var awardedAchievements = new List<string>();
        ulong totalExp = 0;

        var enabledAchievements = config != null
            ? systemAchievements.Where(x => config.AchievementEnabled.ContainsKey(x.Achievement))
            : systemAchievements;

        foreach (var a in enabledAchievements)
        {
            var result = a.Achieve(action, config, tasks);

            if (result.IsAchieved)
            {
                await userAchievementRepository.CreateOrUpdateAsync(userId, a.Achievement);
                awardedAchievements.Add(a.Achievement.ToString());
                totalExp += result.Exp;
            }
        }

        return new AchievementResult(string.Join(", ", awardedAchievements), totalExp);
    }
}