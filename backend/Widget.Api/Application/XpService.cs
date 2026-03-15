using System.Collections.Immutable;
using Widget.Api.ApiModels;
using Widget.Api.Repositories;

namespace Widget.Api.Application;

public class XpService(
    ProjectConfigurationRepository projectConfigurationRepository,
    UserRepository userRepository,
    LevelCalculator levelCalculator,
    UserAchievementRepository userAchievementRepository,
    HeroClassesService heroClassesService)
{
    public record XpAddResult(ulong Exp, ulong ExpChange, int? LevelUpgradedTo, string? HeroClass);

    public async Task<XpAddResult> TryAddXp(PostEventApiModel eventApiModel, ulong achievementReward)
    {
        if (eventApiModel.Event is EventType.ISSUE_RESOLVED or EventType.BUG_RESOLVED &&
            eventApiModel.Children?.Length is null or 0)
        {
            var userAchievements = await userAchievementRepository.GetByUserIdAsync(eventApiModel.Login);
            var user = await userRepository.GetUserById(eventApiModel.Login);
            
            if (user == null)
            {
                var heroClass = heroClassesService.CalculateHeroClasses(user?.Level ?? 0,
                    userAchievements.Select(x => x.Achievement).ToImmutableList());
                
                user = new UserRepository.UserItem(eventApiModel.Login, 0, 0, heroClass, DateTime.UtcNow);
            }
            
            var cfg = await projectConfigurationRepository.GetByProjectIdAsync(eventApiModel.ProjectKey);

            if (cfg is null )
                return new(0, 0, null, null);
            
            var taskXp = 0;
            
            if (eventApiModel.Event == EventType.ISSUE_RESOLVED)
                taskXp += cfg.IssueResolveReward;
            else if (eventApiModel.Event == EventType.BUG_RESOLVED)
                taskXp += cfg.BugResolveReward; 

            if (cfg.IssueWeightType == IssueWeightType.StoryPoints &&
                int.TryParse(eventApiModel.StoryPoints, out var storyPoints))
            {
                taskXp = storyPoints * cfg.IssueUnitWeight;
            }
            else if (cfg.IssueWeightType == IssueWeightType.Time && !string.IsNullOrEmpty(eventApiModel.StoryPoints))
            {
                var timeSpan = ParseTimeSpan(eventApiModel.StoryPoints);
                if (timeSpan.TotalHours > 0)
                    taskXp = (int)timeSpan.TotalHours * cfg.IssueUnitWeight;
            }

            if (cfg.PriorityMultipliers.TryGetValue(eventApiModel.IssuePriority, out var priorityMultiplier))
                taskXp = (int)(priorityMultiplier * taskXp);
            
            taskXp += (int)achievementReward;
            
            var newXp = user.Xp + (ulong)taskXp;

            int? levelUpgraded = null;
            var actualLevenInfo = levelCalculator.GetLevelInfo(newXp);
            if (actualLevenInfo.Level > user.Level)
                levelUpgraded = actualLevenInfo.Level;
            
            var newUserClass = heroClassesService.CalculateHeroClasses(actualLevenInfo.Level, userAchievements.Select(x => x.Achievement).ToImmutableList());
            await userRepository.SetXpAndLevel(user.Id, newXp, actualLevenInfo.Level, newUserClass);

            return new(newXp, (ulong)taskXp, levelUpgraded, newUserClass);
        }

        return new(0, 0, null, null);
    }

    private TimeSpan ParseTimeSpan(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return TimeSpan.Zero;

        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var total = TimeSpan.Zero;

        foreach (var part in parts)
        {
            if (part.Length < 2) continue;

            var unit = part[^1];
            if (!double.TryParse(part[..^1], out var value)) continue;

            total += unit switch
            {
                'd' => TimeSpan.FromDays(value),
                'h' => TimeSpan.FromHours(value),
                'm' => TimeSpan.FromMinutes(value),
                _ => TimeSpan.Zero
            };
        }

        return total;
    }
}