using MongoDB.Driver;
using Widget.Api.Application;

namespace Widget.Api.Repositories;

public class ProjectConfigurationRepository(IMongoDatabase database)
{
    private readonly IMongoCollection<ProjectConfiguration> _collection =
        database.GetCollection<ProjectConfiguration>("ProjectConfigurations");

    public async Task<ProjectConfiguration?> GetByProjectIdAsync(string projectId, CancellationToken ct = default)
    {
        return await _collection.Find(pc => pc.ProjectId == projectId)
            .FirstOrDefaultAsync(cancellationToken: ct);
    }

    public async Task UpsertAsync(ProjectConfiguration configuration, CancellationToken ct = default)
    {
        await _collection.ReplaceOneAsync(
            pc => pc.ProjectId == configuration.ProjectId,
            configuration,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken: ct);
    }
}
