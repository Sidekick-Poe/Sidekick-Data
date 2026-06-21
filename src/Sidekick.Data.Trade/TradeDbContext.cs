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
    public DbSet<TradeItemCategory> ItemCategories { get; init; }
    public DbSet<TradeStat> Stats  { get; init; }
    public DbSet<TradeStatOption> StatOptions  { get; init; }
    public DbSet<TradeStatCategory> StatCategories  { get; init; }
    public DbSet<TradeStaticItem> StaticItems  { get; init; }
    public DbSet<TradeStaticItemCategory> StaticItemCategories  { get; init; }
    public DbSet<Models.TradeFilter> Filters  { get; init; }
    public DbSet<TradeLeague> Leagues  { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TradeItem>()
            .HasOne(i => i.Category)
            .WithMany()
            .HasForeignKey(i => new { i.Game, i.Language, i.CategoryId })
            .HasPrincipalKey(c => new { c.Game, c.Language, c.Id });

        modelBuilder.Entity<TradeStat>()
            .HasOne(s => s.Category)
            .WithMany()
            .HasForeignKey(s => new { s.Game, s.Language, s.CategoryId })
            .HasPrincipalKey(c => new { c.Game, c.Language, c.Id });

        modelBuilder.Entity<TradeStaticItem>()
            .HasOne(s => s.Category)
            .WithMany()
            .HasForeignKey(s => new { s.Game, s.Language, s.CategoryId })
            .HasPrincipalKey(c => new { c.Game, c.Language, c.Id });

        modelBuilder.Entity<TradeStat>()
            .HasMany(s => s.Options)
            .WithOne()
            .HasForeignKey(o => new { o.Game, o.Language, o.StatId })
            .HasPrincipalKey(s => new { s.Game, s.Language, s.Id });
    }
}