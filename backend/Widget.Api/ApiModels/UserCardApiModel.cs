namespace Widget.Api.ApiModels;

public record UserAchievementApiModel(
    int Id,
    string ImageUrl,
    string Description,
    int Count);

public record UserCardApiModel(
    ulong Xp,
    ulong MaxXp,
    int Level,
    IEnumerable<UserAchievementApiModel> Achievements,
    string HeroClass);
