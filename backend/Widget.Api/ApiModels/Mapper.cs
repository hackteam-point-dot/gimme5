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
            Achievement.Sheeva => new UserAchievementApiModel((int)type,
                "https://widget-back-ghh6fve6c7hxamfv.westeurope-01.azurewebsites.net/pngspixelart/sheeva.png",
                "Выдается за 4 задачи одновременно в прогрессе.", level),
            _ => new UserAchievementApiModel((int)type, "", type.ToString(), level)
        };
    }
}