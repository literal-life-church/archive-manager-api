using LiteralLifeChurch.ArchiveManagerApi.Authentication.Domain.UseCase;
using LiteralLifeChurch.ArchiveManagerApi.DI.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Extensions;

internal static class ConfigurationServiceCollectionExtensions
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services)
    {
        services
            .AddSingleton<IGetAuthenticatedClientUseCase, GetAuthenticatedClientUseCase>();

        return services;
    }

    public static IServiceCollection AddFactories(this IServiceCollection services)
    {
        services
            .AddSingleton<IGraphServiceClientFactory, GraphServiceClientFactory>()
            .AddSingleton<ITokenCredentialOptionsFactory, TokenCredentialOptionsFactory>()
            .AddSingleton<IUsernamePasswordCredentialFactory, UsernamePasswordCredentialFactory>();

        return services;
    }
}
