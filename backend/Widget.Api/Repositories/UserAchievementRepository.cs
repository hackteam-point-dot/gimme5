using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Widget.Api.Domain;

namespace Widget.Api.Repositories;

public class UserAchievementRepository(IMongoDatabase database)
{
    public record UserAchievementItem(
        [property: BsonId]
        [property: BsonRepresentation(BsonType.ObjectId)]
        string UserId,
        Achievement Achievement,
        ulong Xp,
        DateTime DateCreated);

    private readonly IMongoCollection<UserAchievementItem> _collection =
        database.GetCollection<UserAchievementItem>("UserAchievements");

    public async Task<UserAchievementItem> CreateAsync(string userId, Achievement achievement, ulong xp, CancellationToken ct = default)
    {
        var document = new UserAchievementItem(userId, achievement, xp, DateTime.UtcNow);
        await _collection.InsertOneAsync(document, cancellationToken: ct);
        return document;
    }

    public async Task<UserAchievementItem?> IncrementXpAsync(string userId, ulong amount, CancellationToken ct = default)
    {
        var filter = Builders<UserAchievementItem>.Filter.Eq(u => u.UserId, userId);
        var update = Builders<UserAchievementItem>.Update.Inc(u => u.Xp, amount);

        return await _collection.FindOneAndUpdateAsync(
            filter,
            update,
            new FindOneAndUpdateOptions<UserAchievementItem>
            {
                IsUpsert = false,
                ReturnDocument = ReturnDocument.After
            },
            cancellationToken: ct);
    }
}