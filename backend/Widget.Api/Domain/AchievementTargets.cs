using System.Collections.Immutable;
using Widget.Api.Domain.Targets;

namespace Widget.Api.Domain;

public static class AchievementTargets
{
    public static ImmutableDictionary<Achievement, ITarget> UserTargets { get; } =
        new Dictionary<Achievement, ITarget>
        {
            {
                Achievement.BugSlayer, new BugSlayerTarget()
            }
        }.ToImmutableDictionary();
}