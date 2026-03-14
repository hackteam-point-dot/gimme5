using Microsoft.AspNetCore.Mvc;
using Widget.Api.ApiModels;
using Widget.Api.Application;
using Widget.Api.Repositories;

namespace Widget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController(UserRepository userRepository, LevelCalculator levelCalculator) : ControllerBase
{
    [HttpGet]
    public IActionResult OkResult()
    {
        return Ok("ok");
    }

    [HttpGet("card")]
    public async Task<ActionResult<UserCardApiModel>> GetUserCard([FromQuery]string userId)
    {
        var user = await userRepository.GetUserById(userId);

        UserCardApiModel card;

        if (user == null)
        {
            var firstLevel = levelCalculator.FirstLevelInfo;
            card = new UserCardApiModel(0, firstLevel.MaxXp, firstLevel.Level, []);
        }
        else
        {
            var levelInfo = levelCalculator.GetLevelInfo(user.Xp);
            card = new UserCardApiModel(
                user.Xp,
                levelInfo.MaxXp,
                user.Level,
                [
                    new UserAchievementApiModel(1, "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAyNCAyNCIgd2lkdGg9IjI0IiBoZWlnaHQ9IjI0IiBmaWxsPSJub25lIiBzdHJva2U9IiMyQzU4NzciIHN0cm9rZS13aWR0aD0iMiIgc3Ryb2tlLWxpbmVjYXA9InJvdW5kIiBzdHJva2UtbGluZWpvaW49InJvdW5kIj4KICA8Y2lyY2xlIGN4PSIxMiIgY3k9IjEyIiByPSIxMCIgZmlsbD0iI0YyRERCRCIvPgogIDxjaXJjbGUgY3g9IjEyIiBjeT0iMTIiIHI9IjYiIHN0cm9rZT0iIzg1QzNENyIvPgogIDxjaXJjbGUgY3g9IjEyIiBjeT0iMTIiIHI9IjIiIGZpbGw9IiNFODgxNDUiIHN0cm9rZT0iI0U4ODE0NSIvPgogIDxsaW5lIHgxPSIxMiIgeTE9IjIiIHgyPSIxMiIgeTI9IjQiIHN0cm9rZT0iIzJDNTg3NyIvPgogIDxsaW5lIHgxPSIxMiIgeTE9IjIwIiB4Mj0iMTIiIHkyPSIyMiIgc3Ryb2tlPSIjMkM1ODc3Ii8+CiAgPGxpbmUgeDE9IjIiIHkxPSIxMiIgeDI9IjQiIHkyPSIxMiIgc3Ryb2tlPSIjMkM1ODc3Ii8+CiAgPGxpbmUgeDE9IjIwIiB5MT0iMTIiIHgyPSIyMiIgeTI9IjEyIiBzdHJva2U9IiMyQzU4NzciLz4KPC9zdmc+", "First Blood — resolved the first issue in the sprint", 1),
                    new UserAchievementApiModel(2, "https://api.dicebear.com/9.x/shapes/svg?seed=speed-demon&size=24", "Speed Demon — closed 5 issues in one day", 3),
                    new UserAchievementApiModel(3, "https://api.dicebear.com/9.x/shapes/svg?seed=bug-hunter&size=24", "Bug Hunter — found and reported 10 bugs", 1),
                    new UserAchievementApiModel(4, "https://api.dicebear.com/9.x/shapes/svg?seed=team-player&size=24", "Team Player — reviewed 15 pull requests", 7),
                    new UserAchievementApiModel(5, "https://api.dicebear.com/9.x/shapes/svg?seed=streak-master&size=24", "Streak Master — completed tasks 7 days in a row", 2)
                ]
            );
        }
        
        return Ok(card);
    }
}
