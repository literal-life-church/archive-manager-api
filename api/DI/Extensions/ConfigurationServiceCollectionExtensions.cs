using LiteralLifeChurch.ArchiveManagerApi.Authentication.Domain.UseCase;
using LiteralLifeChurch.ArchiveManagerApi.DI.Factories;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Data.DataSource;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Mapper;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Repository;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.UseCase;
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
            .AddSingleton<IDriveItemToItemMapper, DriveItemToItemMapper>()
            .AddSingleton<IDriveToDriveIdMapper, DriveToDriveIdMapper>()
            .AddSingleton<IDriveNetworkRepository, DriveNetworkRepository>()
            .AddSingleton<IGetAllSharedFilesUseCase, GetAllSharedFilesUseCase>();

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
