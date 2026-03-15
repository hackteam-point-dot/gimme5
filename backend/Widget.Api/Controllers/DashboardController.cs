using Microsoft.AspNetCore.Mvc;
using Widget.Api.ApiModels;
using Widget.Api.Repositories;

namespace Widget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(UserRepository userRepository) : ControllerBase
{
    [HttpGet("leaderboard")]
    public async Task<ActionResult<UserLeaderboardApiModel>> GetLeaderboard([FromQuery] int limit = 10,
        [FromQuery] int skip = 0,
        CancellationToken ct = default)
    {
        var users = await userRepository.GetLeaderboardAsync(limit, skip, ct);
        var totalCount = await userRepository.GetTotalUsersCount(ct);

        var leaderboard =
            new UserLeaderboardApiModel(users.Select(u => new UserLeaderboardApiModel.Item(u.Id, u.Xp, u.Level)),
                skip, totalCount);

        return Ok(leaderboard);
    }
}