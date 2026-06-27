using Microsoft.EntityFrameworkCore;
using Sidekick.Data.Ninja.Models;

namespace Sidekick.Data.Ninja;

public sealed class NinjaDbContext : DbContext
{
    private static bool _hasMigrated;

    public NinjaDbContext(DbContextOptions<NinjaDbContext> options)
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
}
