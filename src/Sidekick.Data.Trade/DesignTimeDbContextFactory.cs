using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sidekick.Data.Trade;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TradeDbContext>
{
    public TradeDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<TradeDbContext>();
        builder.UseSqlite("Filename=../../data/trade.db");
        return new TradeDbContext(builder.Options);
    }
}
