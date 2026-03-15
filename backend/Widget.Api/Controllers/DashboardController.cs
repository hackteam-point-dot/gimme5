using Microsoft.AspNetCore.Mvc;
using Widget.Api.ApiModels;
using Widget.Api.Repositories;

namespace Widget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(
    UserRepository userRepository,
    UserAchievementRepository userAchievementRepository,
    LeaderboardRepository leaderboardRepository)
    : ControllerBase
{
    [HttpGet("leaderboard")]
    public async Task<ActionResult<UserLeaderboardApiModel>> GetLeaderboard(
        [FromQuery] string? projectId,
        [FromQuery] int limit = 10,
        [FromQuery] int skip = 0,
        CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(projectId))
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
                return new UserLeaderboardApiModel.Item(u.Id, u.Xp, u.Level, achievements, u.Title);
            });

            var leaderboard = new UserLeaderboardApiModel(items, skip, totalCount);

            return Ok(leaderboard);
        }
        else
        {
            var currentPeriod = await leaderboardRepository.GetCurrentPeriodAsync(projectId, ct) ??
                                await leaderboardRepository.StartNewPeriod(projectId, ct);
            
            var users = await leaderboardRepository.GetLeaderboard(currentPeriod.Id, limit, skip, ct);
            var totalCount = await leaderboardRepository.GetTotalUsersCount(currentPeriod.Id, ct);

            var userAchievementLevelsByUserId = users
                .ToDictionary(
                    x => x.Key.UserId,
                    x => x.Achievements.GroupBy(y => y).ToDictionary(y => y.Key, y => y.Count()));

            var items = users.Select(async u =>
            {
                var user = await userRepository.GetUserById(u.Key.UserId, ct);
                var achievements = userAchievementLevelsByUserId.TryGetValue(u.Key.UserId, out var levels)
                        ? levels.Select(x => Mapper.MapUserAchievementApiModel(x.Key, x.Value)).ToArray()
                        : []
                    ;
                return new UserLeaderboardApiModel.Item(u.Key.UserId, u.Exp, 0, achievements, user?.Title ?? string.Empty);
            }).Select(x => x.Result);

            var leaderboard = new UserLeaderboardApiModel(items, skip, totalCount);

            return Ok(leaderboard);
        }
    }
    

    [HttpPost("leaderboard/reset")]
    public async Task<IActionResult> ResetLeaderboard([FromQuery] string projectId, CancellationToken ct = default)
    {
        var currentPeriod = await leaderboardRepository.GetCurrentPeriodAsync(projectId, ct);
        if (currentPeriod is not null)
            await leaderboardRepository.ClosePeriod(currentPeriod.Id, ct);
        return Ok();
    }
}