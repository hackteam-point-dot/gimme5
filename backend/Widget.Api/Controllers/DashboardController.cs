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
            var users = await userRepository.GetLeaderboard(limit, skip, ct);
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
            var currentPeriod = await leaderboardRepository.GetCurrentPeriod(projectId, ct) ??
                                await leaderboardRepository.StartNewPeriod(projectId, ct);
            
            var leaderboard = await LeaderBoardForPeriod(currentPeriod, limit, skip, ct);

            return Ok(leaderboard);
        }
    }

    [HttpGet("leaderboard/previous")]
    public async Task<ActionResult<UserLeaderboardApiModel>> GetLeaderboardForPreviousPeriod(
        [FromQuery] string projectId,
        [FromQuery] int limit = 10,
        [FromQuery] int skip = 0,
        CancellationToken ct = default)
    {
        var previousPeriod = await leaderboardRepository.GetPreviousPeriod(projectId, ct);
        
        if (previousPeriod is null)
            return new UserLeaderboardApiModel([], skip, 0);

        var leaderboard = await LeaderBoardForPeriod(previousPeriod, limit, skip, ct);

        return Ok(leaderboard);
    }

    private async Task<UserLeaderboardApiModel> LeaderBoardForPeriod(LeaderboardPeriod previousPeriod, int limit,
        int skip, CancellationToken ct)
    {
        var leaderboardItems = await leaderboardRepository.GetLeaderboard(previousPeriod.Id, limit, skip, ct);
        var totalCount = await leaderboardRepository.GetTotalUsersCount(previousPeriod.Id, ct);

        var userIds = leaderboardItems.Select(x => x.Key.UserId).ToArray();
        var users = await userRepository.GetUsersByIds(userIds, ct);
        var usersMap = users.ToDictionary(x => x.Id);

        var userAchievementLevelsByUserId = leaderboardItems
            .ToDictionary(
                x => x.Key.UserId,
                x => x.Achievements.GroupBy(y => y).ToDictionary(y => y.Key, y => y.Count()));

        var items = leaderboardItems.Select(async u =>
        {
            var user = usersMap.GetValueOrDefault(u.Key.UserId);
            var achievements = userAchievementLevelsByUserId.TryGetValue(u.Key.UserId, out var levels)
                ? levels.Select(x => Mapper.MapUserAchievementApiModel(x.Key, x.Value)).ToArray()
                : [];

            return new UserLeaderboardApiModel.Item(u.Key.UserId, u.Exp, user?.Level ?? 0, achievements,
                user?.Title ?? string.Empty);
        }).Select(x => x.Result);

        var leaderboard = new UserLeaderboardApiModel(items, skip, totalCount);
        return leaderboard;
    }

    [HttpPost("leaderboard/reset")]
    public async Task<IActionResult> ResetLeaderboard([FromQuery] string projectId, CancellationToken ct = default)
    {
        var currentPeriod = await leaderboardRepository.GetCurrentPeriod(projectId, ct);
        if (currentPeriod is not null)
            await leaderboardRepository.ClosePeriod(currentPeriod.Id, ct);
        return Ok();
    }
}