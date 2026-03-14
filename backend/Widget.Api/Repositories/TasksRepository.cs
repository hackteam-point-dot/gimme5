using MongoDB.Driver;
using Widget.Api.ApiModels;

namespace Widget.Api.Repositories;

public class TasksRepository(IMongoDatabase database)
{
    public record TaskItem(
        string Id,
        string ProjectId,
        string CreatorId,
        int StoryPoints,
        EventType Status,
        string? ResolverId);

    private readonly IMongoCollection<TaskItem> _collection =
        database.GetCollection<TaskItem>("Tasks");

    public async Task<List<TaskItem>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken ct = default)
    {
        return await _collection.Find(t => ids.Contains(t.Id))
            .ToListAsync(cancellationToken: ct);
    }
}