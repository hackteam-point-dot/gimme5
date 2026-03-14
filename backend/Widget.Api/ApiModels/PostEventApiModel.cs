namespace Widget.Api.ApiModels;

public record PostEventApiModel(EventType Event, string IssueId, string Login, string ProjectKey, string ProjectName, string[] Children);