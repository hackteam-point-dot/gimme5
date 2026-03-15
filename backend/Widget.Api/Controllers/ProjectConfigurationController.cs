using Microsoft.AspNetCore.Mvc;
using Widget.Api.ApiModels;
using Widget.Api.Application;
using Widget.Api.Repositories;

namespace Widget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectConfigurationController(ProjectConfigurationRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ProjectConfigurationApiModel>> GetConfiguration([FromQuery] string projectId)
    {
        var config = await repository.GetByProjectIdAsync(projectId);

        if (config == null)
        {
            return NotFound();
        }

        return Ok(new ProjectConfigurationApiModel(
            config.ProjectId,
            config.IssueWeightType,
            config.IssueUnitWeight,
            config.IssueResolveReward,
            config.BugResolveReward,
            config.IssueWeightFieldName,
            config.PriorityMultipliers,
            config.AchievementRewards,
            config.AchievementEnabled));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateConfiguration([FromBody] ProjectConfigurationApiModel model)
    {
        await repository.UpsertAsync(new ProjectConfiguration
        {
            ProjectId = model.ProjectId,
            IssueWeightType = model.IssueWeightType,
            IssueUnitWeight = model.IssueUnitWeight,
            IssueResolveReward = model.IssueResolveReward,
            BugResolveReward = model.BugResolveReward,
            IssueWeightFieldName = model.IssueWeightFieldName,
            PriorityMultipliers = model.PriorityMultipliers,
            AchievementRewards = model.AchievementRewards
        });

        return NoContent();
    }
}
