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
    public DbSet<TradeStatCategory> StatCategories { get; init; }
    public DbSet<TradeStatOption> StatOptions  { get; init; }
    public DbSet<TradeStaticItem> StaticItems  { get; init; }
    public DbSet<TradeStaticItemCategory> StaticItemCategories { get; init; }
    public DbSet<Models.TradeFilter> Filters  { get; init; }
    public DbSet<Models.TradeFilterCategory> FilterCategories { get; init; }
    public DbSet<Models.TradeFilterOption> FilterOptions  { get; init; }
    public DbSet<TradeLeague> Leagues  { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TradeItemCategory>()
            .HasMany(c => c.Items)
            .WithOne(i => i.Category)
            .HasForeignKey(i => i.CategoryId)
            .HasPrincipalKey(c => c.UniqueId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TradeStatCategory>()
            .HasMany(c => c.Stats)
            .WithOne(s => s.Category)
            .HasForeignKey(s => s.CategoryId)
            .HasPrincipalKey(c => c.UniqueId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TradeStaticItemCategory>()
            .HasMany(c => c.StaticItems)
            .WithOne(s => s.Category)
            .HasForeignKey(s => s.CategoryId)
            .HasPrincipalKey(c => c.UniqueId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Models.TradeFilterCategory>()
            .HasMany(c => c.Children)
            .WithOne(f => f.Category)
            .HasForeignKey(f => f.CategoryId)
            .HasPrincipalKey(c => c.UniqueId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Models.TradeFilter>()
            .HasMany(f => f.Options)
            .WithOne(o=> o.TradeFilter)
            .HasForeignKey(o => o.FilterUniqueId)
            .HasPrincipalKey(f => f.UniqueId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TradeStat>()
            .HasMany(f => f.Options)
            .WithOne(o => o.TradeStat)
            .HasForeignKey(o => o.TradeStatUniqueId)
            .HasPrincipalKey(f => f.UniqueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}