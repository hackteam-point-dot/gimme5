using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Widget.Api.Domain;

namespace Widget.Api.Repositories;

public class UserAchievementRepository(IMongoDatabase database)
{
    public record UserAchievementKey(string UserId, Achievement Achievement);
    
    [BsonIgnoreExtraElements]
    public record UserAchievementItem(
        [property: BsonId]
        UserAchievementKey Key,
        Achievement Achievement,
        int Level,
        DateTime DateCreated);

    private readonly IMongoCollection<UserAchievementItem> _collection =
        database.GetCollection<UserAchievementItem>("UserAchievements");

    public async Task<UserAchievementItem> CreateOrUpdate(string userId, Achievement achievement, CancellationToken cancellationToken = default)
    {
        var key = new UserAchievementKey(userId, achievement);

        var filter = Builders<UserAchievementItem>.Filter.Eq(x => x.Key, key);

        var update = Builders<UserAchievementItem>.Update
            .Inc(x => x.Level, 1)
            .SetOnInsert(x => x.Key, key)
            .SetOnInsert(x => x.Achievement, achievement)
            .SetOnInsert(x => x.DateCreated, DateTime.UtcNow);

        var options = new FindOneAndUpdateOptions<UserAchievementItem>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };

        return await _collection.FindOneAndUpdateAsync(filter, update, options, cancellationToken);
    }
    
    public async Task<List<UserAchievementItem>> GetByUserId(string userId, CancellationToken ct = default)
    {
        return await _collection.Find(x => x.Key.UserId == userId)
            .ToListAsync(ct);
    }

    public async Task<List<UserAchievementItem>> GetByUserIds(string[] userIds, CancellationToken ct = default)
    {
        return await _collection.Find(x => userIds.Contains(x.Key.UserId)).ToListAsync(ct);
    }
}