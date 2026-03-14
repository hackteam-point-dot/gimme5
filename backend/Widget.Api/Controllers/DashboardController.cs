using Microsoft.AspNetCore.Mvc;
using Widget.Api.Repositories;

namespace Widget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(UserRepository userRepository) : ControllerBase
{
    [HttpGet("leaderboard/{projectId}")]
    public async Task<IActionResult> GetLeaderboard(string projectId, [FromQuery] int limit = 10, CancellationToken ct = default)
    {
        var leaderboard = await userRepository.GetLeaderboardAsync(projectId, limit, ct);
        return Ok(leaderboard);
    }

    [HttpGet]
    public IActionResult OkResult()
    {
        return Ok("ok");
    }
}
