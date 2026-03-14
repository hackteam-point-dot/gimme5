using MongoDB.Driver;
using Widget.Api.Models;

namespace Widget.Api.Repositories;

public class TestRepository
{
    private readonly IMongoCollection<TestDocument> _collection;

    public TestRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<TestDocument>("TestCollection");
    }

    public async Task<TestDocument> CreateAsync(string name, CancellationToken cancellationToken = default)
    {
        var document = new TestDocument
        {
            Name = name,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _collection.InsertOneAsync(document, cancellationToken: cancellationToken);
        return document;
    }

    public async Task<IReadOnlyCollection<TestDocument>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var documents = await _collection
            .Find(_ => true)
            .ToListAsync(cancellationToken);

        return documents;
    }
}
