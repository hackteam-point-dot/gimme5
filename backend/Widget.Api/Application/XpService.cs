using Widget.Api.ApiModels;
using Widget.Api.Repositories;

namespace Widget.Api.Application;

public class XpService(TasksRepository tasksRepository)
{
    public async Task TryAddXp(PostEventApiModel eventApiModel)
    {
        if (eventApiModel.Event == EventType.ISSUE_RESOLVED && eventApiModel.Children?.Length is null or 0)
        {
            var taskXp = eventApiModel.StoryPoints * 10;
            
        }
    }
}