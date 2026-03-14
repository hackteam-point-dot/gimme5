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
        string ResolverId,
        bool ExpAwarded,
        ImmutableList<string> SubTaskIds);

    private readonly IMongoCollection<TaskItem> _collection =
        database.GetCollection<TaskItem>("Tasks");

    public async Task<List<TaskItem>> GetAllAsync(CancellationToken ct = default)
    {
        return await _collection.Find(FilterDefinition<TaskItem>.Empty)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<List<TaskItem>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken ct = default)
    {
        return await _collection.Find(t => ids.Contains(t.Id))
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<TaskItem?> CreateOrUpdateAsync(TaskItem item, CancellationToken ct = default)
    {
        var update = Builders<TaskItem>.Update
            .Set(t => t.ProjectId, item.ProjectId)
            .Set(t => t.Status, item.Status)
            .Set(t => t.ResolverId, item.ResolverId)
            .Set(t => t.SubTaskIds, item.SubTaskIds)
            .SetOnInsert(t => t.ExpAwarded, item.ExpAwarded);

        return await _collection.FindOneAndUpdateAsync(
            t => t.Id == item.Id,
            update,
            new FindOneAndUpdateOptions<TaskItem>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            },
            cancellationToken: ct);
    }

    public async Task<TaskItem?> SetExpAwardedAsync(string id, bool expAwarded, CancellationToken ct = default)
    {
        var update = Builders<TaskItem>.Update
            .Set(t => t.ExpAwarded, expAwarded);

        return await _collection.FindOneAndUpdateAsync(
            t => t.Id == id,
            update,
            new FindOneAndUpdateOptions<TaskItem>
            {
                ReturnDocument = ReturnDocument.After
            },
            cancellationToken: ct);
    }
}