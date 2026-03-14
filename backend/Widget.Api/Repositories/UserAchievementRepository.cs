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
        DateTime DateCreated);

    private readonly IMongoCollection<UserAchievementItem> _collection =
        database.GetCollection<UserAchievementItem>("UserAchievements");

    public async Task<UserAchievementItem> CreateAsync(string userId, Achievement achievement, CancellationToken ct = default)
    {
        var document = new UserAchievementItem(userId, achievement, DateTime.UtcNow);
        await _collection.InsertOneAsync(document, cancellationToken: ct);
        return document;
    }
}