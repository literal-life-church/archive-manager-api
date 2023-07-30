using LiteralLifeChurch.ArchiveManagerApi.Authentication.Domain.UseCase;
using LiteralLifeChurch.ArchiveManagerApi.DI.Factories;
using LiteralLifeChurch.ArchiveManagerApi.DI.Forwarders;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Data.DataSource;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Mapper;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Data.Repository;
using LiteralLifeChurch.ArchiveManagerApi.Drive.Domain.UseCase;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Data.Mapper;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Data.Repository;
using LiteralLifeChurch.ArchiveManagerApi.Extraction.Domain.UseCase;
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
            .AddSingleton<IDriveItemToFileMapper, DriveItemToFileMapper>()
            .AddSingleton<IDriveToDriveIdMapper, DriveToDriveIdMapper>()
            .AddSingleton<IDriveNetworkRepository, DriveNetworkRepository>()
            .AddSingleton<IGetAllSharedFilesUseCase, GetAllSharedFilesUseCase>();

        return services;
    }

    public static IServiceCollection AddExtraction(this IServiceCollection services)
    {
        services
            .AddSingleton<IFileToMediaMetadataMapper, FileToMediaMetadataMapper>()
            .AddSingleton<IMediaMetadataMemoryRepository, MediaMetadataMemoryRepository>()
            .AddSingleton<IExtractMediaMetadataFromFilesUseCase, ExtractMediaMetadataFromFilesUseCase>();

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

    public static IServiceCollection AddForwarders(this IServiceCollection services)
    {
        services
            .AddSingleton<IDateTimeForwarder, DateTimeForwarder>();

        return services;
    }
}
