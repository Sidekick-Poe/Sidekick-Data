using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Sidekick.Data.Trade.Models;

namespace Sidekick.Data.Trade;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TradeDbContext>
{
    public TradeDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<TradeDbContext>();
        builder.UseSqlite("Filename=trade.db");
        return new TradeDbContext(builder.Options);
    }
}
