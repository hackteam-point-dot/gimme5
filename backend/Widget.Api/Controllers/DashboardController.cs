using Microsoft.AspNetCore.Mvc;
using Widget.Api.ApiModels;
using Widget.Api.Repositories;

namespace Widget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(UserRepository userRepository, UserAchievementRepository userAchievementRepository)
    : ControllerBase
{
    [HttpGet("leaderboard")]
    public async Task<ActionResult<UserLeaderboardApiModel>> GetLeaderboard([FromQuery] int limit = 10,
        [FromQuery] int skip = 0,
        CancellationToken ct = default)
    {
        var users = await userRepository.GetLeaderboardAsync(limit, skip, ct);
        var totalCount = await userRepository.GetTotalUsersCount(ct);

        var userIds = users.Select(u => u.Id).ToArray();
        var userAchievements = await userAchievementRepository.GetByUserIds(userIds, ct);
        var userAchievementLevelsByUserId = userAchievements
            .GroupBy(x => x.Key.UserId)
            .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.Achievement, y => y.Level));

        var items = users.Select(u =>
        {
            var achievements = userAchievementLevelsByUserId.TryGetValue(u.Id, out var levels)
                ? levels.Select(x => Mapper.MapUserAchievementApiModel(x.Key, x.Value)).ToArray()
                : []
            ;
            return new UserLeaderboardApiModel.Item(u.Id, u.Xp, u.Level, achievements);
        });
        
        var leaderboard = new UserLeaderboardApiModel(items, skip, totalCount);

        return Ok(leaderboard);
    }
}