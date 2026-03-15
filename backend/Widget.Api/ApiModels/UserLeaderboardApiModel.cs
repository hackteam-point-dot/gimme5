namespace Widget.Api.ApiModels;

public record UserLeaderboardApiModel(IEnumerable<UserLeaderboardApiModel.Item> Items, int Offset, long TotalCount)
{
    public record Item(string UserId, ulong Exp, int Level, UserAchievementApiModel[] Achievements, string HeroClass);
}
