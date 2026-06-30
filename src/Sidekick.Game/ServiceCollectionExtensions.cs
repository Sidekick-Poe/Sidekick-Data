using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sidekick.Common;
using Sidekick.Data.Fuzzy;
using Sidekick.Data.Languages;

namespace Sidekick.Game;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSidekickGame(
        this IServiceCollection services)
    {
        services.AddSidekickInitializableService<ICurrentGameLanguage, CurrentGameLanguage>();
        services.AddSingleton<IGameLanguageProvider, GameLanguageProvider>();

        services.TryAddSingleton<IFuzzyService, FuzzyService>();

        return services;
    }
}
