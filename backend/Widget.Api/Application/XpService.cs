using Widget.Api.ApiModels;

namespace Widget.Api.Application;

public class XpService
{
    public async Task TryAddXp(PostEventApiModel eventApiModel)
    {
        if (eventApiModel is { Event: EventType.STORY_DONE, StoryPoints: > 0 })
        {
            
        }
    }
}