using LiteralLifeChurch.ArchiveManagerApi.Authentication.Domain.UseCase;
using LiteralLifeChurch.ArchiveManagerApi.DI.Factories;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Data.DataSource;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Mapper;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Repository;
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

    public static IServiceCollection AddDrive(this IServiceCollection services)
    {
        services
            .AddSingleton<IDriveNetworkDataSource, DriveNetworkDataSource>()
            .AddSingleton<IDriveIdToStringMapper, DriveIdToStringMapper>()
            .AddSingleton<IDriveNetworkRepository, DriveNetworkRepository>();

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
