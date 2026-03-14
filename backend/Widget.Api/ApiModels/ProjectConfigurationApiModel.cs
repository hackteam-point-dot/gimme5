using Widget.Api.Application;

namespace Widget.Api.ApiModels;

public record ProjectConfigurationApiModel(
    string ProjectId,
    IssueWeightType IssueWeightType,
    int DefaultIssueWeight,
    int IssueUnitWeight,
    string IssueWeightFieldName);