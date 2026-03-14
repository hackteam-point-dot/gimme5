using MongoDB.Driver;

namespace Widget.Api.Repositories;

public class UserRepository(IMongoDatabase database)
{
    public record UserItem(
        string Id,
        ulong Xp,
        DateTime DateCreated);

    public record UserProjectKey(string UserId, string ProjectId);

    public record UserProjectItem(
        UserProjectKey Id,
        ulong Balance);

    private readonly IMongoCollection<UserItem> _usersCollection = database.GetCollection<UserItem>("Users");

    private readonly IMongoCollection<UserProjectItem> _userProjectsCollection =
        database.GetCollection<UserProjectItem>("UserProjects");

    // public async Task<UserItem> CreateAsync(string userId, string projectId, ulong balance, ulong xp,
    //     CancellationToken ct = default)
    // {
    //     var document = new UserItem(userId, xp, DateTime.UtcNow);
    //     await _usersCollection.InsertOneAsync(document, cancellationToken: ct);
    //     return document;
    // }
    
    public async Task<UserItem?> GetUserById(string userId, CancellationToken ct = default)
    {
        return await _usersCollection.Find(x => x.Id == userId).FirstOrDefaultAsync(ct);
    }

    public async Task<List<UserProjectItem>> GetLeaderboardAsync(string projectId, int limit = 10,
        CancellationToken ct = default)
    {
        return await _userProjectsCollection.Find(u => u.Id.ProjectId == projectId)
            .SortByDescending(u => u.Balance)
            .Limit(limit)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<UserItem?> IncrementXpAsync(string userId, ulong amount, CancellationToken ct = default)
    {
        var filter = Builders<UserItem>.Filter.Eq(u => u.Id, userId);
        var update = Builders<UserItem>.Update.Inc(u => u.Xp, amount);

        return await _usersCollection.FindOneAndUpdateAsync(
            filter,
            update,
            new FindOneAndUpdateOptions<UserItem>
            {
                IsUpsert = false,
                ReturnDocument = ReturnDocument.After
            },
            cancellationToken: ct);
    }
}