using System.Text;
using Widget.Api.ApiModels;
using Widget.Api.Domain.Targets;
using Widget.Api.Repositories;

namespace Widget.Api.Application;

public class AchievementService(
    TasksRepository tasksRepository,
    IEnumerable<ITarget> achievements,
    UserAchievementRepository userAchievementRepository,
    ProjectConfigurationRepository projectConfigurationRepository)
{
    public record AchievementResult(string AchievementName, ulong Exp);
    
    public async Task<AchievementResult> CalculateAchievements(PostEventApiModel action, string userId)
    {
        var tasks = await tasksRepository.GetAllAsync();
        //var config = projectConfigurationRepository.GetByProjectIdAsync()
        var awardedAchievements = new List<string>();
        ulong totalExp = 0;

        foreach (var a in achievements)
        {
            var result = a.Achieve(action, tasks);

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