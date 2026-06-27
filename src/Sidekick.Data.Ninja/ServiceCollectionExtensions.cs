using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Common;

namespace Sidekick.Data.Ninja;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSidekickDataNinja(this IServiceCollection services)
    {
        var path = Path.Combine(SidekickPaths.GetDataDirectory(), "ninja.db");
        services.AddDbContextPool<NinjaDbContext>(o => o.UseSqlite("Data Source=" + path));

        return services;
    }
}
