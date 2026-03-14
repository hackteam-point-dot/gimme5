using Widget.Api.ApiModels;
using Widget.Api.Repositories;
using Widget.Api.Domain;

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
                IssueUnitWeight = 10,
                IssueWeightFieldName = "Story points",
                BugResolveReward = 70,
                IssueResolveReward = 50,
                PriorityMultipliers = new Dictionary<Priority, decimal>
                {
                    { Priority.Minor, 1.0m },
                    { Priority.Normal, 1.5m },
                    { Priority.Major, 2.0m },
                    { Priority.Critical, 3.0m }
                },
                AchievementRewards = new()
                {
                    [Achievement.TaskBuilder] = 50,
                    [Achievement.OnFire] = 200,
                    [Achievement.DeadlineHero] = 200,
                    [Achievement.BugHunter] = 100,
                    [Achievement.NightOwl] = 20,
                }
                
            }, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
