namespace Widget.Api.ApiModels;

public record EventApiResponse(ulong Exp, ulong ExpChange, int? LevelUpgradedTo, string? Achievement, ulong AchievementExp);