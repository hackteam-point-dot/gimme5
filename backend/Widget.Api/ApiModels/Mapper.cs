using Widget.Api.Domain;

namespace Widget.Api.ApiModels;

public static class Mapper
{
    public static UserAchievementApiModel MapUserAchievementApiModel(Achievement type, int level)
    {
        return type switch
        {
            Achievement.TaskBuilder => new UserAchievementApiModel((int)type,
                "https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net/pngspixelart/task-builder.png",
                "Moving any single Task or User Story to Done.", level),
            Achievement.DeadlineHero => new UserAchievementApiModel((int)type,
                "https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net/pngspixelart/deadline-hero.png",
                "No tasks of yours received the 'Carry Over' status (moved to the next sprint) during the entire sprint.", level),
            Achievement.OnFire => new UserAchievementApiModel((int)type,
                "https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net/pngspixelart/on-fire.png",
                "Close at least one task for 5 consecutive working days within the current sprint.", level),
            Achievement.BugHunter => new UserAchievementApiModel((int)type,
                "https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net/pngspixelart/bug-hunter.png",
                "Cumulative achievement. Awarded for every 5 bugs closed in total.", level),
            Achievement.NightOwl => new UserAchievementApiModel((int)type,
                "https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net/pngspixelart/night-owl.png",
                "Awarded for closing a task during non-standard hours (before 09:00 or after 21:00 according to the work schedule).", level),
            Achievement.Sheeva => new UserAchievementApiModel((int)type,
                "https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net/pngspixelart/sheeva.png",
                "Awarded for having 4 tasks in progress simultaneously.", level),
            Achievement.EasterAgg => new UserAchievementApiModel((int)type,
                "https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net/pngspixelart/egg.png",
                "Well done, you found it!", level),
            _ => new UserAchievementApiModel((int)type, "", type.ToString(), level)
        };
    }
}