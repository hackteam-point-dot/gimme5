using Widget.Api.Application;

namespace Widget.Api.ApiModels;

public record ProjectConfigurationApiModel(
    string ProjectId,
    IssueWeightType IssueWeightType,
    int DefaultIssueWeight,
    int IssueUnitWeight,
    int IssueResolveReward,
    int BugResolveReward,
    string IssueWeightFieldName,
    Dictionary<Widget.Api.Application.Priority, decimal> PriorityMultipliers,
    Dictionary<Widget.Api.Domain.Achievement, int> AchievementRewards);