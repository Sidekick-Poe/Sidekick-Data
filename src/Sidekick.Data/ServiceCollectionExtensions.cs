using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Common;

namespace Sidekick.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSidekickData(
        this IServiceCollection services)
    {
        var path = Path.Combine(SidekickPaths.GetDataDirectory(), "data.db");
        services.AddDbContextPool<DataDbContext>(o => o.UseSqlite("Data Source=" + path));

        return services;
    }
}