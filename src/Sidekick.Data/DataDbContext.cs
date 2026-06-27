using Microsoft.EntityFrameworkCore;
using Sidekick.Data.Ninja;
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

    public DbSet<NinjaExchangeItem> NinjaExchangeItems => Set<NinjaExchangeItem>();
    public DbSet<NinjaStashItem> NinjaStashItems => Set<NinjaStashItem>();
    public DbSet<NinjaStashTradeStat> NinjaStashTradeStats => Set<NinjaStashTradeStat>();
    public DbSet<NinjaStashMutatedStat> NinjaStashMutatedStats => Set<NinjaStashMutatedStat>();

    public DbSet<TradeLeague> TradeLeagues  { get; init; }
    public DbSet<TradeItem> TradeItems { get; init; }
    public DbSet<TradeItemCategory> TradeItemCategories { get; init; }
    public DbSet<TradeStat> TradeStats  { get; init; }
    public DbSet<TradeStatCategory> TradeStatCategories { get; init; }
    public DbSet<TradeStaticItem> TradeStaticItems  { get; init; }
    public DbSet<TradeStaticItemCategory> TradeStaticItemCategories { get; init; }
    public DbSet<TradeFilter> TradeFilters  { get; init; }
    public DbSet<TradeFilterCategory> TradeFilterCategories { get; init; }
    public DbSet<TradeFilterOption> TradeFilterOptions  { get; init; }
}