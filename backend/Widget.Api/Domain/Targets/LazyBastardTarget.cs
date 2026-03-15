namespace Widget.Api.Domain.Targets;

public class LazyBastardTarget : ISecretTarget
{
    public AchievementResult IsAchieved(string userId, int score)
    {
        return score > 2 
            ? new AchievementResult(true, 300, []) 
            : AchievementResult.NoResult;
    }

    public Achievement Achievement => Achievement.EasterAgg;
}