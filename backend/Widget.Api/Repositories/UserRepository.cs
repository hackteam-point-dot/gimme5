using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Widget.Api.Domain;

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
}