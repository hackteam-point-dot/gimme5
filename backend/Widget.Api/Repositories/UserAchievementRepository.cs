using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Widget.Api.Domain;

namespace Widget.Api.Repositories;

public class UserAchievementRepository(IMongoDatabase database)
{
    public record UserAchievementItem(
        [property: BsonId]
        string Id,
        Achievement Achievement,
        int Level,
        DateTime DateCreated);

    private readonly IMongoCollection<UserAchievementItem> _collection =
        database.GetCollection<UserAchievementItem>("UserAchievements");

    public async Task<UserAchievementItem> CreateOrUpdateAsync(string userId, Achievement achievement, CancellationToken cancellationToken = default)
    {
        var id = $"{userId}:{achievement}";

        var filter = Builders<UserAchievementItem>.Filter.Eq(x => x.Id, id);

        var update = Builders<UserAchievementItem>.Update
            .Inc(x => x.Level, 1)
            .SetOnInsert(x => x.Id, id)
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