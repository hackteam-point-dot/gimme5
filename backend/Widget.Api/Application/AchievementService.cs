using System.Text;
using Widget.Api.ApiModels;
using Widget.Api.Domain;
using Widget.Api.Domain.Targets;
using Widget.Api.Repositories;

namespace Widget.Api.Application;

public class AchievementService(
    TasksRepository tasksRepository,
    IEnumerable<ITarget> systemAchievements,
    IEnumerable<ISecretTarget> secretAchievements,
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
            var result = a.IsAchieved(action, config, tasks.Where(x => x.ResolverId == userId).ToList());

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

    public async Task<bool> CalculateLazyBastardAchievement(string userId, int score)
    {
        foreach (var achievement in secretAchievements)
        {
            var result = achievement.IsAchieved(userId, score);
            if (result.IsAchieved)
            {
                await userAchievementRepository.CreateOrUpdateAsync(userId, achievement.Achievement);
                return result.IsAchieved;
            }
        }

        return false;
    }
}