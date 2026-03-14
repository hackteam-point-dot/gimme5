namespace Widget.Api.Application;

public class ProjectConfiguration
{
    public string ProjectId { get; init; } = string.Empty;
    public IssueWeightType IssueWeightType { get; init; }
    public int DefaultIssueWeight { get; init; }
    public int IssueUnitWeight { get; init; }
    public string IssueWeightFieldName { get; init; } = "Story Points";
}

public enum IssueWeightType
{
    StoryPoints,
    Time
}