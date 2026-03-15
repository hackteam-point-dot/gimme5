using System.Collections.Immutable;
using Widget.Api.Domain;

namespace Widget.Api.Application;

public class HeroClassesService
{
    private readonly Achievement[] _achievementsRequired =
        [Achievement.BugHunter, Achievement.OnFire, Achievement.DeadlineHero];
    
    public string CalculateHeroClasses(int currentLevel, ImmutableList<Achievement> achievements)
    {
        var hasRequiredAchievements = _achievementsRequired.All(achievements.Contains);

        if (!hasRequiredAchievements)
            return currentLevel switch
            {
                0 => "Newbie",
                1 => "Contributor",
                2 => "Solver",
                3 => "Specialist",
                4 => "Expert",
                5 => "Legend",
                _ => string.Empty
            };

        return currentLevel switch
        {
            0 => "Newbie",
            1 => "Rookie",
            2 => "Blaze Tender",
            3 => "Inferno Wrangler",
            4 => "Pyro Vanguard",
            5 => "Flame Conqueror",
            _ => string.Empty
        };
    }
}