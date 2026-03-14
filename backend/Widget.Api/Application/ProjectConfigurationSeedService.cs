using Widget.Api.Repositories;
using Widget.Api.Application;

namespace Widget.Api.Application;

public class ProjectConfigurationSeedService(IServiceProvider serviceProvider) : IHostedService
{
    private const string ProjectId = "SCR";

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<ProjectConfigurationRepository>();

        var config = await repository.GetByProjectIdAsync(ProjectId, cancellationToken);

        if (config == null)
        {
            await repository.UpsertAsync(new ProjectConfiguration
            {
                ProjectId = ProjectId,
                IssueWeightType = IssueWeightType.StoryPoints,
                DefaultIssueWeight = 10,
                IssueUnitWeight = 10,
                IssueWeightFieldName = "Story points"
            }, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
