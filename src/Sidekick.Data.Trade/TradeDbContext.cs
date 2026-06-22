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

    public DbSet<TradeItem> Items { get; init; }
    public DbSet<TradeStat> Stats  { get; init; }
    public DbSet<TradeStatOption> StatOptions  { get; init; }
    public DbSet<TradeStaticItem> StaticItems  { get; init; }
    public DbSet<Models.TradeFilter> Filters  { get; init; }
    public DbSet<Models.TradeFilterOption> FilterOptions  { get; init; }
    public DbSet<TradeLeague> Leagues  { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.TradeFilter>()
            .HasMany(f => f.Options)
            .WithOne()
            .HasForeignKey(o => new { o.Game, o.Language, o.FilterId })
            .HasPrincipalKey(f => new { f.Game, f.Language, f.Id });

        modelBuilder.Entity<TradeStat>()
            .HasMany(f => f.Options)
            .WithOne()
            .HasForeignKey(o => new { o.Game, o.Language, o.StatId })
            .HasPrincipalKey(f => new { f.Game, f.Language, f.Id });
    }
}