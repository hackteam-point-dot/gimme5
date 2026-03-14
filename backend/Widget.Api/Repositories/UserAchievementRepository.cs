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
        int Level,
        DateTime DateCreated);

    private readonly IMongoCollection<UserAchievementItem> _collection =
        database.GetCollection<UserAchievementItem>("UserAchievements");

    public async Task<UserAchievementItem> CreateOrUpdateAsync(string userId, Achievement achievement, CancellationToken cancellationToken = default)
    {
        var filter = Builders<UserAchievementItem>.Filter.And(
            Builders<UserAchievementItem>.Filter.Eq(x => x.UserId, userId),
            Builders<UserAchievementItem>.Filter.Eq(x => x.Achievement, achievement));

        var update = Builders<UserAchievementItem>.Update
            .Inc(x => x.Level, 1)
            .SetOnInsert(x => x.UserId, userId)
            .SetOnInsert(x => x.Achievement, achievement)
            .SetOnInsert(x => x.DateCreated, DateTime.UtcNow);

        var options = new FindOneAndUpdateOptions<UserAchievementItem>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };

        return await _collection.FindOneAndUpdateAsync(filter, update, options, cancellationToken);
    }
}