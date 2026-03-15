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
        string Title,
        DateTime DateCreated);

    private readonly IMongoCollection<UserItem> _usersCollection = database.GetCollection<UserItem>("Users");
    
    public async Task<UserItem?> GetUserById(string userId, CancellationToken ct = default)
    {
        return await _usersCollection.Find(x => x.Id == userId).FirstOrDefaultAsync(ct);
    }

    public async Task<List<UserItem>> GetLeaderboardAsync(int limit = 10, int skip = 0, CancellationToken ct = default)
    {
        return await _usersCollection.Find(u => true)
            .SortByDescending(u => u.Xp)
            .Skip(skip)
            .Limit(limit)
            .ToListAsync(cancellationToken: ct);
    }
    
    public async Task<long> GetTotalUsersCount(CancellationToken ct = default)
    {
        return await _usersCollection.CountDocumentsAsync(u => true, cancellationToken: ct);
    }

    public async Task<UserItem?> SetXpAndLevel(string userId, ulong xp, int level, string title,
        CancellationToken ct = default)
    {
        var filter = Builders<UserItem>.Filter.Eq(u => u.Id, userId);
        var update = Builders<UserItem>.Update
            .Set(u => u.Xp, xp)
            .Set(u => u.Level, level);

        if (!string.IsNullOrEmpty(title))
            update.Set(x => x.Title, title);

        return await _usersCollection.FindOneAndUpdateAsync(
            filter,
            update,
            new FindOneAndUpdateOptions<UserItem>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            },
            cancellationToken: ct);
    }
}