using Widget.Api.ApiModels;

namespace Widget.Api.Domain.Targets;

public class TaskBuilderTarget(EventType eventType) : ITarget
{
    private const int Bonus = 50;
    
    public bool IsAchieved()
    {
        return eventType is EventType.ISSUE_RESOLVED or EventType.BUG_RESOLVED;
    }
}