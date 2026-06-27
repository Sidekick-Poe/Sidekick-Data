using Sidekick.Data.Stats;
using Sidekick.Data.StatsInvariant;
using Sidekick.Data.Trade;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Cli.StatsInvariant;

public class StatsInvariantBuilder(
    DbContextOptions<DataDbContext> dbContextOptions)
{
    public async Task Build(GameType game)
    {
        await using var db = new DataDbContext(dbContextOptions);
        var stats = await db.TradeStats
            .Where(s => s.Game == game && s.Language == "en")
            .ToListAsync();

        // Clear existing entries for this game/language
        db.StatsInvariantIgnore.RemoveRange(db.StatsInvariantIgnore.Where(s => s.Game == game));
        db.StatsInvariantFireWeaponDamage.RemoveRange(db.StatsInvariantFireWeaponDamage.Where(s => s.Game == game));
        db.StatsInvariantColdWeaponDamage.RemoveRange(db.StatsInvariantColdWeaponDamage.Where(s => s.Game == game));
        db.StatsInvariantLightningWeaponDamage.RemoveRange(
            db.StatsInvariantLightningWeaponDamage.Where(s => s.Game == game));
        db.StatsInvariantIncursionRoom.RemoveRange(db.StatsInvariantIncursionRoom.Where(s => s.Game == game));
        db.StatsInvariantLogbookFaction.RemoveRange(db.StatsInvariantLogbookFaction.Where(s => s.Game == game));
        db.StatsInvariantLogbookBoss.RemoveRange(db.StatsInvariantLogbookBoss.Where(s => s.Game == game));
        await db.SaveChangesAsync();

        // Insert new entries
        foreach (var id in GetIgnoreStatIds(stats))
        {
            db.StatsInvariantIgnore.Add(new StatsInvariantIgnore
            {
                SidekickId = Guid.NewGuid(),
                Game = game,
                StatId = id,
            });
        }

        foreach (var id in GetFireWeaponDamageIds(stats))
        {
            db.StatsInvariantFireWeaponDamage.Add(new StatsInvariantFireWeaponDamage
            {
                SidekickId = Guid.NewGuid(),
                Game = game,
                StatId = id,
            });
        }

        foreach (var id in GetColdWeaponDamageIds(stats))
        {
            db.StatsInvariantColdWeaponDamage.Add(new StatsInvariantColdWeaponDamage
            {
                SidekickId = Guid.NewGuid(),
                Game = game,
                StatId = id,
            });
        }

        foreach (var id in GetLightningWeaponDamageIds(stats))
        {
            db.StatsInvariantLightningWeaponDamage.Add(new StatsInvariantLightningWeaponDamage
            {
                SidekickId = Guid.NewGuid(),
                Game = game,
                StatId = id,
            });
        }

        foreach (var id in GetIncursionRooms(stats))
        {
            db.StatsInvariantIncursionRoom.Add(new StatsInvariantIncursionRoom
            {
                SidekickId = Guid.NewGuid(),
                Game = game,
                StatId = id,
            });
        }

        foreach (var id in GetLogbookFactions(stats))
        {
            db.StatsInvariantLogbookFaction.Add(new StatsInvariantLogbookFaction
            {
                SidekickId = Guid.NewGuid(),
                Game = game,
                StatId = id,
            });
        }

        foreach (var id in GetLogbookBosses(stats))
        {
            db.StatsInvariantLogbookBoss.Add(new StatsInvariantLogbookBoss
            {
                SidekickId = Guid.NewGuid(),
                Game = game,
                StatId = id,
            });
        }

        await db.SaveChangesAsync();
    }

    private IEnumerable<string> GetIgnoreStatIds(List<TradeStat> stats)
    {
        foreach (var stat in stats)
        {
            if (stat.Id.GetStatCategory() != StatCategory.Pseudo) continue;
            if (stat.Text?.StartsWith("#% chance for dropped Maps to convert to") == true) yield return stat.Id;
        }
    }

    private IEnumerable<string> GetFireWeaponDamageIds(List<TradeStat> stats)
    {
        foreach (var stat in stats)
        {
            if (stat.Id.GetStatCategory() == StatCategory.Pseudo) continue;
            if (stat.Text == "Adds # to # Fire Damage") yield return stat.Id;
        }
    }

    private IEnumerable<string> GetColdWeaponDamageIds(List<TradeStat> stats)
    {
        foreach (var stat in stats)
        {
            if (stat.Id.GetStatCategory() == StatCategory.Pseudo) continue;
            if (stat.Text == "Adds # to # Cold Damage") yield return stat.Id;
        }
    }

    private IEnumerable<string> GetLightningWeaponDamageIds(List<TradeStat> stats)
    {
        foreach (var stat in stats)
        {
            if (stat.Id.GetStatCategory() == StatCategory.Pseudo) continue;
            if (stat.Text == "Adds # to # Lightning Damage") yield return stat.Id;
        }
    }

    private IEnumerable<string> GetIncursionRooms(List<TradeStat> stats)
    {
        foreach (var stat in stats)
        {
            if (stat.Id.GetStatCategory() != StatCategory.Pseudo) continue;
            if (stat.Text?.StartsWith("Has Room: ") == true && stat.Id.GetStatOption() != 2) yield return stat.Id;
        }
    }

    private IEnumerable<string> GetLogbookFactions(List<TradeStat> stats)
    {
        foreach (var stat in stats)
        {
            if (stat.Id.GetStatCategory() != StatCategory.Pseudo) continue;
            if (stat.Text?.StartsWith("Has Logbook Faction: ") == true) yield return stat.Id;
        }
    }

    private IEnumerable<string> GetLogbookBosses(List<TradeStat> stats)
    {
        foreach (var stat in stats)
        {
            if (stat.Id.GetStatCategory() != StatCategory.Implicit) continue;
            if (stat.Text == "Area contains an Expedition Boss (#)") yield return stat.Id;
        }
    }
}