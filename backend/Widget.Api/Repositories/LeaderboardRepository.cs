using MongoDB.Bson;
using MongoDB.Driver;
using Widget.Api.Domain;

namespace Widget.Api.Repositories;

public record LeaderboardPeriod(ObjectId Id, string ProjectId, bool IsCurrent, DateTime Start, DateTime? End);

public record LeaderboardItemKey(ObjectId PeriodId, string UserId);

public record LeaderboardItem(LeaderboardItemKey Key, ulong Exp, Achievement[] Achievements);

public class LeaderboardRepository(IMongoDatabase database)
{
    private readonly IMongoCollection<LeaderboardPeriod> _periodsCollection =
        database.GetCollection<LeaderboardPeriod>("LeaderboardPeriods");

    private readonly IMongoCollection<LeaderboardItem> _itemsCollection =
        database.GetCollection<LeaderboardItem>("LeaderboardItems");

    public async Task<LeaderboardPeriod?> GetCurrentPeriodAsync(string projectId, CancellationToken ct = default)
    {
        return await _periodsCollection.Find(p => p.ProjectId == projectId && p.IsCurrent).FirstOrDefaultAsync(ct);
    }

    public async Task ClosePeriod(ObjectId periodId, CancellationToken ct = default)
    {
        await _periodsCollection.UpdateOneAsync(
            x => x.Id == periodId,
            Builders<LeaderboardPeriod>.Update
                .Set(x => x.End, DateTime.UtcNow)
                .Set(x => x.IsCurrent, false),
            cancellationToken: ct);
    }

    public async Task<LeaderboardPeriod> StartNewPeriod(string projectId, CancellationToken ct = default)
    {
        var period = new LeaderboardPeriod(ObjectId.GenerateNewId(), projectId, true, DateTime.UtcNow, null);
        await _periodsCollection.InsertOneAsync(period, cancellationToken: ct);
        return period;
    }

    public async Task IncrementExpAndAchievement(ObjectId periodId, string userId, ulong exp,
        Achievement[] achievements, CancellationToken ct = default)
    {
        var key = new LeaderboardItemKey(periodId, userId);

        var update = Builders<LeaderboardItem>.Update
            .SetOnInsert(x => x.Key, key)
            .Inc(x => x.Exp, exp)
            .PushEach(x => x.Achievements, achievements);

        await _itemsCollection.UpdateOneAsync(x => x.Key == key, update, new UpdateOptions { IsUpsert = true },
            cancellationToken: ct);
    }
}