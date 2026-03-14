namespace Widget.Api.Application;

public record LevelInfo(int Level, ulong MinXp, ulong MaxXp);

public class LevelCalculator
{
    private static readonly List<ulong> FixedLevels = new()
    {
        0,          // Level 1: 0 - 500
        500,      // Level 2: 500 - 1200
        1200,     // Level 3: 1200 - 2200
        2200,     // Level 4: 2200 - 3500
        3500,     // Level 5: 3500 - 5200
        5200,     // Level 6: 5200 - 7400
        7400,     // Level 7: 7400 - 10200
        10200,    // Level 8: 10200 - 13700
        13700,    // Level 9: 13700 - 18000
        18000// Level 10 (Legend): 18000+
    };

    private const ulong LegendStep = 5000;

    public LevelInfo FirstLevelInfo => new (1, 0, FixedLevels[1]);
    
    public LevelInfo GetLevelInfo(ulong totalXp)
    {
        if (totalXp < FixedLevels[^1])
        {
            for (int i = 0; i < FixedLevels.Count - 1; i++)
            {
                if (totalXp >= FixedLevels[i] && totalXp < FixedLevels[i + 1])
                {
                    return new LevelInfo(
                        i + 1,
                        FixedLevels[i],
                        FixedLevels[i + 1]);
                }
            }
        }

        // Legend levels (10+)
        ulong legendXp = totalXp - FixedLevels[^1];
        int extraLevels = (int)(legendXp / LegendStep);
        int currentLevel = 10 + extraLevels;
        
        ulong currentLevelMinXp = FixedLevels[^1] + (ulong)extraLevels * LegendStep;
        ulong nextLevelMaxXp = currentLevelMinXp + LegendStep;
        
        // Reward for reaching Level 10 is 1000.
        // For levels > 10, let's assume the reward is also 1000 or follows some logic?
        // Description says: "(Далее прогрессия повторяется с шагом +5000 XP за каждый последующий уровень)".
        // It doesn't explicitly say if reward repeats or increases. Usually it repeats or stays same as level 10.
        // Let's assume reward is 1000 for level 10 and each subsequent level.
        
        return new LevelInfo(
            currentLevel,
            currentLevelMinXp,
            nextLevelMaxXp);
    }
}
