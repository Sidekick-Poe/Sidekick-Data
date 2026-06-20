using Microsoft.Extensions.DependencyInjection;

namespace Sidekick.Data.Cli;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSidekickDataBuilder(
        this IServiceCollection services)
    {
        return services;
    }
}
