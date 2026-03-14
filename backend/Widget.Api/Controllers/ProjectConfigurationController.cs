using Microsoft.AspNetCore.Mvc;
using Widget.Api.ApiModels;
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
            config.DefaultIssueWeight,
            config.IssueUnitWeight,
            config.IssueWeightFieldName));
    }
}
