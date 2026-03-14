using Widget.Api.ApiModels;
using Widget.Api.Repositories;

namespace Widget.Api.Application;

public class XpService(ProjectConfigurationRepository projectConfigurationRepository, UserRepository userRepository)
{
    public record XpAddResult(ulong Exp, ulong ExpChange);
    
    public async Task<XpAddResult> TryAddXp(PostEventApiModel eventApiModel)
    {
        if (eventApiModel.Event == EventType.ISSUE_RESOLVED && eventApiModel.Children?.Length is null or 0)
        {
            var cfg = await projectConfigurationRepository.GetByProjectIdAsync(eventApiModel.ProjectKey);
            
            var user = await userRepository.GetUserById(eventApiModel.Login);
            
            if (cfg is null || user is null)
                return new (0, 0);

            int taskXp = cfg.DefaultIssueWeight;

            if (cfg.IssueWeightType == IssueWeightType.StoryPoints &&
                int.TryParse(eventApiModel.StoryPoints, out var storyPoints))
            {
                taskXp = storyPoints * cfg.IssueUnitWeight;
            }
            else if (cfg.IssueWeightType == IssueWeightType.Time && !string.IsNullOrEmpty(eventApiModel.StoryPoints))
            {
                var timeSpan = ParseTimeSpan(eventApiModel.StoryPoints);
                if (timeSpan.TotalHours > 0)
                    taskXp = (int)timeSpan.TotalHours * cfg.IssueUnitWeight;
            }

            return new(user.Xp, (ulong)taskXp);
        }
        
        return new (0, 0);
    }

    private TimeSpan ParseTimeSpan(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return TimeSpan.Zero;

        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var total = TimeSpan.Zero;

        foreach (var part in parts)
        {
            if (part.Length < 2) continue;

            var unit = part[^1];
            if (!double.TryParse(part[..^1], out var value)) continue;

            total += unit switch
            {
                'd' => TimeSpan.FromDays(value),
                'h' => TimeSpan.FromHours(value),
                'm' => TimeSpan.FromMinutes(value),
                _ => TimeSpan.Zero
            };
        }

        return total;
    }
}