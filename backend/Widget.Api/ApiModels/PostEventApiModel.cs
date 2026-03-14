namespace Widget.Api.ApiModels;

public record PostEventApiModel(EventType Event, string UserId, string? TaskId);