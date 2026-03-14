using Widget.Api.ApiModels;
using Widget.Api.Repositories;

namespace Widget.Api.Application;

public class XpService(TasksRepository tasksRepository)
{
    public async Task TryAddXp(PostEventApiModel eventApiModel)
    {
        if (eventApiModel is { Event: EventType.STORY_DONE, StoryPoints: > 0, Children.Length: > 0 })
        {
            var tasks = await tasksRepository.GetByIdsAsync(eventApiModel.Children);
            
            var totalXp = eventApiModel.StoryPoints * 10;
            var tasksByUser = tasks.GroupBy(t => t.AssigneeId);
        }
    }
}