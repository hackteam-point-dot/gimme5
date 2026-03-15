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
    [HttpGet]
    public IActionResult OkResult()
    {
        return Ok("ok");
    }

    [HttpGet("card")]
    public async Task<ActionResult<UserCardApiModel>> GetUserCard([FromQuery] string userId)
    {
        var user = await userRepository.GetUserById(userId);
        var userAchievements = await userAchievementRepository.GetByUserIdAsync(userId);
        
        var userAchievementLevels = userAchievements.ToDictionary(x => x.Achievement, x => x.Level);

        UserCardApiModel card;

        var achievements = Enum.GetValues<Achievement>()
            .Select(achievementType =>
            {
                var level = userAchievementLevels.GetValueOrDefault(achievementType, 0);
                return CreateUserAchievementApiModel(achievementType, level);
            })
            .ToArray();

        if (user == null)
        {
            var firstLevel = levelCalculator.FirstLevelInfo;
            card = new UserCardApiModel(0, firstLevel.MaxXp, firstLevel.Level, achievements);
        }
        else
        {
            var levelInfo = levelCalculator.GetLevelInfo(user.Xp);
            card = new UserCardApiModel(user.Xp, levelInfo.MaxXp, user.Level, achievements);
        }

        return Ok(card);
    }

    private static UserAchievementApiModel CreateUserAchievementApiModel(Achievement type, int level)
    {
        return type switch
        {
            Achievement.TaskBuilder => new UserAchievementApiModel((int)type,
                "https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net/pngspixelart/task-builder.png",
                "Перевод одной любой задачи (Task) или User Story в Done.", level),
            Achievement.DeadlineHero => new UserAchievementApiModel((int)type,
                "https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net/pngspixelart/deadline-hero.png",
                "За весь спринт ни одна ваша задача не получила статус \"Carry Over\" (сдвинута на следующий спринт).", level),
            Achievement.OnFire => new UserAchievementApiModel((int)type,
                "https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net/pngspixelart/on-fire.png",
                "Закрывать хотя бы по одной задаче 5 рабочих дней подряд внутри текущего спринта.", level),
            Achievement.BugHunter => new UserAchievementApiModel((int)type,
                "https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net/pngspixelart/bug-hunter.png",
                "Накапливаемая ачивка. Выдается за каждые 5 суммарно закрытых багов.", level),
            Achievement.NightOwl => new UserAchievementApiModel((int)type,
                "https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net/pngspixelart/night-owl.png",
                "Выдается за закрытие задачи в нестандартные часы (до 09:00 или после 21:00 по рабочему графику).", level),
            _ => new UserAchievementApiModel((int)type, "", type.ToString(), level)
        };
    }
    
}