using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Common;

namespace Sidekick.Data.Trade;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSidekickDataTrade(this IServiceCollection services)
    {
        var path = Path.Combine(SidekickPaths.GetDataDirectory(), "trade.db");
        services.AddDbContextPool<TradeDbContext>(o => o.UseSqlite("Data Source=" + path));

        return services;
    }
}
