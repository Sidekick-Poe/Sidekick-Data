using Microsoft.EntityFrameworkCore;
using Sidekick.Data.Trade.Models;

namespace Sidekick.Data.Trade;

public sealed class TradeDbContext : DbContext
{
    private static bool _hasMigrated;

    public TradeDbContext(DbContextOptions<TradeDbContext> options)
        : base(options)
    {
        if (_hasMigrated) return;

        Database.Migrate();
        _hasMigrated = true;
    }

    public DbSet<TradeLeague> TradeLeagues  { get; init; }
    public DbSet<TradeItem> TradeItems { get; init; }
    public DbSet<TradeItemCategory> TradeItemCategories { get; init; }
    public DbSet<TradeStat> TradeStats  { get; init; }
    public DbSet<TradeStatCategory> TradeStatCategories { get; init; }
    public DbSet<TradeStaticItem> TradeStaticItems  { get; init; }
    public DbSet<TradeStaticItemCategory> TradeStaticItemCategories { get; init; }
    public DbSet<Models.TradeFilter> TradeFilters  { get; init; }
    public DbSet<Models.TradeFilterCategory> TradeFilterCategories { get; init; }
    public DbSet<Models.TradeFilterOption> TradeFilterOptions  { get; init; }
}