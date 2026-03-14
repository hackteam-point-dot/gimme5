using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Widget.Api.Repositories;

public class UserRepository(IMongoDatabase database)
{
    [BsonIgnoreExtraElements]
    public record UserItem(
        string Id,
        ulong Xp,
        int Level,
        DateTime DateCreated);

    public record UserProjectKey(string UserId, string ProjectId);

    [BsonIgnoreExtraElements]
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

    public async Task<UserItem?> SetXpAndLevel(string userId, ulong xp, int level, CancellationToken ct = default)
    {
        var filter = Builders<UserItem>.Filter.Eq(u => u.Id, userId);
        var update = Builders<UserItem>.Update
            .Set(u => u.Xp, xp)
            .Set(u => u.Level, level);

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