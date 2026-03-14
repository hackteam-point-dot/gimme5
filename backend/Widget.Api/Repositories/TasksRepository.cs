using MongoDB.Driver;
using Widget.Api.ApiModels;

namespace Widget.Api.Repositories;

public class TasksRepository(IMongoDatabase database)
{
    public record TaskItem(
        string Id,
        string ProjectId,
        string? ParentTaskId,
        string AssigneeId,
        int StoryPoints,
        EventType Status);

    private readonly IMongoCollection<TaskItem> _collection =
        database.GetCollection<TaskItem>("Tasks");

    public async Task<List<TaskItem>> GetSubtasksAsync(string parentTaskId, CancellationToken ct = default)
    {
        return await _collection.Find(t => t.ParentTaskId == parentTaskId)
            .ToListAsync(cancellationToken: ct);
    }
}