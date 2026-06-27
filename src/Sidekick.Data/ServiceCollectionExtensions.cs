using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sidekick.Common;
using Sidekick.Data.Fuzzy;
using Sidekick.Data.Languages;

namespace Sidekick.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSidekickData(
        this IServiceCollection services)
    {
        services.AddSidekickInitializableService<ICurrentGameLanguage, CurrentGameLanguage>();

        services.AddSingleton<IGameLanguageProvider, GameLanguageProvider>();

        services.TryAddSingleton<IFuzzyService, FuzzyService>();

        var path = Path.Combine(SidekickPaths.GetDataDirectory(), "data.db");
        services.AddDbContextPool<DataDbContext>(o => o.UseSqlite("Data Source=" + path));

        return services;
    }
}