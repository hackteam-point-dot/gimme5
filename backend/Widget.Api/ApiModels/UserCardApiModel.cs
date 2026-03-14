namespace Widget.Api.ApiModels;

public record UserAchievementApiModel(
    int Id,
    string ImageUrl,
    string Description,
    int Count);

public record UserCardApiModel(
    int Xp,
    int MaxXp,
    int Level,
    int Balance,
    UserAchievementApiModel[] Achievements);
