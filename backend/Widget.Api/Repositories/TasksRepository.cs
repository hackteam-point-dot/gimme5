using System.Collections.Immutable;
using MongoDB.Driver;
using Widget.Api.ApiModels;

namespace Widget.Api.Repositories;

public class TasksRepository(IMongoDatabase database)
{
    public record TaskItem(
        string Id,
        string ProjectId,
        EventType Status,
        string? ResolverId,
        ImmutableList<string> SubTaskIds);

    private readonly IMongoCollection<TaskItem> _collection =
        database.GetCollection<TaskItem>("Tasks");

    public async Task<List<TaskItem>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken ct = default)
    {
        return await _collection.Find(t => ids.Contains(t.Id))
            .ToListAsync(cancellationToken: ct);
    }

    public async Task CreateOrUpdateAsync(TaskItem item, CancellationToken ct = default)
    {
        await _collection.ReplaceOneAsync(
            t => t.Id == item.Id,
            item,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken: ct);
    }
}