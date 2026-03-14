using Widget.Api.Application;
using Widget.Api.Domain;

namespace Widget.Api.ApiModels;

public record ProjectConfigurationApiModel(
    string ProjectId,
    IssueWeightType IssueWeightType,
    int IssueUnitWeight,
    int IssueResolveReward,
    int BugResolveReward,
    string IssueWeightFieldName,
    Dictionary<Priority, decimal> PriorityMultipliers,
    Dictionary<Achievement, int> AchievementRewards);