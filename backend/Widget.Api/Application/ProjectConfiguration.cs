using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Widget.Api.ApiModels;
using Widget.Api.Domain;

namespace Widget.Api.Application;

[BsonIgnoreExtraElements]
public class ProjectConfiguration
{
    [BsonId]
    public string ProjectId { get; init; } = string.Empty;
    public IssueWeightType IssueWeightType { get; init; }
    public int IssueUnitWeight { get; init; }
    public int IssueResolveReward { get; init; }
    public int BugResolveReward { get; init; }
    public string IssueWeightFieldName { get; init; } = "Story Points";
    
    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
    public Dictionary<Priority, decimal> PriorityMultipliers { get; init; } = new();
    
    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
    public Dictionary<Achievement, int> AchievementRewards { get; init; } = new();
    
    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
    public Dictionary<Achievement, bool> AchievementEnabled { get; init; } = new();

    public bool HideUnreachedAchievements { get; set; }
}

public enum IssueWeightType
{
    None,
    StoryPoints,
    Time
}