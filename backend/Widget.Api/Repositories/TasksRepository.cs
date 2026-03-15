using System.Collections.Immutable;
using MongoDB.Driver;
using Widget.Api.ApiModels;

namespace Widget.Api.Repositories;

public class TasksRepository(IMongoDatabase database)
{
    public enum TaskType
    {
        Issue, Bug
    }
    
    public record TaskItem(
        string Id,
        string ProjectId,
        EventType Status,
        TaskType Type,
        string ResolverId,
        bool ExpAwarded,
        DateTime? DateResolved,
        ImmutableList<string> SubTaskIds);

    private readonly IMongoCollection<TaskItem> _collection =
        database.GetCollection<TaskItem>("Tasks");

    public async Task<List<TaskItem>> GetAll(CancellationToken ct = default)
    {
        return await _collection.Find(FilterDefinition<TaskItem>.Empty)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<List<TaskItem>> GetByIds(IEnumerable<string> ids, CancellationToken ct = default)
    {
        return await _collection.Find(t => ids.Contains(t.Id))
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<TaskItem?> CreateOrUpdate(TaskItem item, CancellationToken ct = default)
    {
        var update = Builders<TaskItem>.Update
            .Set(t => t.ProjectId, item.ProjectId)
            .Set(t => t.Status, item.Status)
            .Set(t => t.ResolverId, item.ResolverId)
            .Set(t => t.SubTaskIds, item.SubTaskIds)
            .Set(t => t.Type, item.Type)
            .Set(t => t.DateResolved, item.DateResolved)
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

    public async Task<TaskItem?> SetExpAwarded(string id, bool expAwarded, CancellationToken ct = default)
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