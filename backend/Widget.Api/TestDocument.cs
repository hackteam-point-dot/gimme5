using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Widget.Api.Models;

public class TestDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
