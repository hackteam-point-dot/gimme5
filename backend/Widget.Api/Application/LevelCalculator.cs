namespace Widget.Api.Application;

public record LevelInfo(int Level, ulong MinXp, ulong MaxXp, ulong Reward);

public class LevelCalculator
{
    private static readonly List<(ulong MinXp, ulong Reward)> FixedLevels = new()
    {
        (0, 0),          // Level 1: 0 - 500
        (500, 100),      // Level 2: 500 - 1200
        (1200, 150),     // Level 3: 1200 - 2200
        (2200, 200),     // Level 4: 2200 - 3500
        (3500, 250),     // Level 5: 3500 - 5200
        (5200, 300),     // Level 6: 5200 - 7400
        (7400, 400),     // Level 7: 7400 - 10200
        (10200, 500),    // Level 8: 10200 - 13700
        (13700, 700),    // Level 9: 13700 - 18000
        (18000, 1000)    // Level 10 (Legend): 18000+
    };

    private const ulong LegendStep = 5000;

    public LevelInfo GetLevelInfo(ulong totalXp)
    {
        if (totalXp < FixedLevels[^1].MinXp)
        {
            for (int i = 0; i < FixedLevels.Count - 1; i++)
            {
                if (totalXp >= FixedLevels[i].MinXp && totalXp < FixedLevels[i + 1].MinXp)
                {
                    return new LevelInfo(
                        i + 1,
                        FixedLevels[i].MinXp,
                        FixedLevels[i + 1].MinXp,
                        FixedLevels[i].Reward);
                }
            }
        }

        // Legend levels (10+)
        ulong legendXp = totalXp - FixedLevels[^1].MinXp;
        int extraLevels = (int)(legendXp / LegendStep);
        int currentLevel = 10 + extraLevels;
        
        ulong currentLevelMinXp = FixedLevels[^1].MinXp + (ulong)extraLevels * LegendStep;
        ulong nextLevelMaxXp = currentLevelMinXp + LegendStep;
        
        // Reward for reaching Level 10 is 1000.
        // For levels > 10, let's assume the reward is also 1000 or follows some logic?
        // Description says: "(Далее прогрессия повторяется с шагом +5000 XP за каждый последующий уровень)".
        // It doesn't explicitly say if reward repeats or increases. Usually it repeats or stays same as level 10.
        // Let's assume reward is 1000 for level 10 and each subsequent level.
        
        return new LevelInfo(
            currentLevel,
            currentLevelMinXp,
            nextLevelMaxXp,
            1000);
    }
}
