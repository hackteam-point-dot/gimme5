using MongoDB.Driver;

namespace Widget.Api.Repositories;

public class UserRepository(IMongoDatabase database)
{
    public record Key(string UserId, string ProjectId);
    public record UserItem(
        Key Id,
        ulong Balance,
        DateTime DateCreated);

    private readonly IMongoCollection<UserItem> _collection =
        database.GetCollection<UserItem>("Users");

    public async Task<UserItem> CreateAsync(string userId, string projectId, ulong balance, CancellationToken ct = default)
    {
        var document = new UserItem(new Key(userId, projectId), balance, DateTime.UtcNow);
        await _collection.InsertOneAsync(document, cancellationToken: ct);
        return document;
    }

    public async Task<List<UserItem>> GetLeaderboardAsync(string projectId, int limit = 10, CancellationToken ct = default)
    {
        return await _collection.Find(u => u.Id.ProjectId == projectId)
            .SortByDescending(u => u.Balance)
            .Limit(limit)
            .ToListAsync(cancellationToken: ct);
    }
}