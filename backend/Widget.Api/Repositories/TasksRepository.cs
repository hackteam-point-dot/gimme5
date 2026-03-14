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

    public async Task<TaskItem?> CreateOrUpdateAsync(TaskItem item, CancellationToken ct = default)
    {
        return await _collection.FindOneAndReplaceAsync(
            t => t.Id == item.Id,
            item,
            new FindOneAndReplaceOptions<TaskItem>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            },
            cancellationToken: ct);
    }
}