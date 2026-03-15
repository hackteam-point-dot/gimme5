using Microsoft.AspNetCore.Mvc;
using Widget.Api.ApiModels;
using Widget.Api.Application;
using Widget.Api.Domain;
using Widget.Api.Repositories;

namespace Widget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController(
    UserRepository userRepository,
    UserAchievementRepository userAchievementRepository,
    LevelCalculator levelCalculator) : ControllerBase
{

    [HttpGet("card")]
    public async Task<ActionResult<UserCardApiModel>> GetUserCard([FromQuery] string userId)
    {
        var user = await userRepository.GetUserById(userId);
        var userAchievements = await userAchievementRepository.GetByUserId(userId);
        
        var userAchievementLevels = userAchievements.ToDictionary(x => x.Achievement, x => x.Level);

        UserCardApiModel card;

        var achievements = Enum.GetValues<Achievement>()
            .Where(x =>  x != Achievement.EasterAgg || userAchievementLevels.ContainsKey(Achievement.EasterAgg))
            .Select(achievementType =>
            {
                var level = userAchievementLevels.GetValueOrDefault(achievementType, 0);
                return Mapper.MapUserAchievementApiModel(achievementType, level);
            })
            .ToList();
        
        if (user == null)
        {
            var firstLevel = levelCalculator.FirstLevelInfo;
            card = new UserCardApiModel(0, firstLevel.MaxXp, firstLevel.Level, achievements, string.Empty);
        }
        else
        {
            var levelInfo = levelCalculator.GetLevelInfo(user.Xp);
            card = new UserCardApiModel(user.Xp, levelInfo.MaxXp, user.Level, achievements, user.Title);
        }

        return Ok(card);
    }
    
}