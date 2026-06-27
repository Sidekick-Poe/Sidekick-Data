using Microsoft.EntityFrameworkCore;
using Sidekick.Data.ItemClasses;
using Sidekick.Data.Ninja;
using Sidekick.Data.StatsInvariant;
using Sidekick.Data.Trade;

namespace Sidekick.Data;

public sealed class DataDbContext : DbContext
{
    private static bool _hasMigrated;

    public DataDbContext(DbContextOptions<DataDbContext> options)
        : base(options)
    {
        if (_hasMigrated) return;

        Database.Migrate();
        _hasMigrated = true;
    }

    public DbSet<ItemClassEntity> ItemClasses => Set<ItemClassEntity>();

    public DbSet<NinjaExchangeItem> NinjaExchangeItems => Set<NinjaExchangeItem>();
    public DbSet<NinjaStashItem> NinjaStashItems => Set<NinjaStashItem>();
    public DbSet<NinjaStashTradeStat> NinjaStashTradeStats => Set<NinjaStashTradeStat>();
    public DbSet<NinjaStashMutatedStat> NinjaStashMutatedStats => Set<NinjaStashMutatedStat>();

    public DbSet<StatsInvariantIgnore> StatsInvariantIgnore => Set<StatsInvariantIgnore>();
    public DbSet<StatsInvariantFireWeaponDamage> StatsInvariantFireWeaponDamage => Set<StatsInvariantFireWeaponDamage>();
    public DbSet<StatsInvariantColdWeaponDamage> StatsInvariantColdWeaponDamage => Set<StatsInvariantColdWeaponDamage>();
    public DbSet<StatsInvariantLightningWeaponDamage> StatsInvariantLightningWeaponDamage => Set<StatsInvariantLightningWeaponDamage>();
    public DbSet<StatsInvariantIncursionRoom> StatsInvariantIncursionRoom => Set<StatsInvariantIncursionRoom>();
    public DbSet<StatsInvariantLogbookFaction> StatsInvariantLogbookFaction => Set<StatsInvariantLogbookFaction>();
    public DbSet<StatsInvariantLogbookBoss> StatsInvariantLogbookBoss => Set<StatsInvariantLogbookBoss>();

    public DbSet<TradeLeague> TradeLeagues => Set<TradeLeague>();
    public DbSet<TradeItem> TradeItems => Set<TradeItem>();
    public DbSet<TradeItemCategory> TradeItemCategories => Set<TradeItemCategory>();
    public DbSet<TradeStat> TradeStats => Set<TradeStat>();
    public DbSet<TradeStatCategory> TradeStatCategories => Set<TradeStatCategory>();
    public DbSet<TradeStaticItem> TradeStaticItems => Set<TradeStaticItem>();
    public DbSet<TradeStaticItemCategory> TradeStaticItemCategories => Set<TradeStaticItemCategory>();
    public DbSet<TradeFilter> TradeFilters => Set<TradeFilter>();
    public DbSet<TradeFilterCategory> TradeFilterCategories => Set<TradeFilterCategory>();
    public DbSet<TradeFilterOption> TradeFilterOptions => Set<TradeFilterOption>();

}
