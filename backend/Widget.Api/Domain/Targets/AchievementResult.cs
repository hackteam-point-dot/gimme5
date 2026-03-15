namespace Widget.Api.Domain.Targets;

public record AchievementResult(bool IsAchieved, ulong Exp)
{
    public static readonly AchievementResult NoResult = new(false, 0);
}