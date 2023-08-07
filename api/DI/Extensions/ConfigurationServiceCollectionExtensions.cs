using LiteralLifeChurch.ArchiveManagerApi.Config.Data.DataSource;
using LiteralLifeChurch.ArchiveManagerApi.Config.Data.Repository;
using LiteralLifeChurch.ArchiveManagerApi.Config.Domain.UseCase;
using LiteralLifeChurch.ArchiveManagerApi.Correlation.Data.Mapper;
using LiteralLifeChurch.ArchiveManagerApi.Correlation.Domain.UseCase;
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
    public static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        services
            .AddSingleton<IAuthenticationOptionsEnvironmentVariableDataSource,
                AuthenticationOptionsEnvironmentVariableDataSource>()
            .AddSingleton<IAuthenticationOptionsEnvironmentVariableRepository,
                AuthenticationOptionsEnvironmentVariableRepository>()
            .AddSingleton<IConfigurationOptionsEnvironmentVariableDataSource,
                ConfigurationOptionsEnvironmentVariableDataSource>()
            .AddSingleton<IConfigurationOptionsEnvironmentVariableRepository,
                ConfigurationOptionsEnvironmentVariableRepository>()
            .AddSingleton<IGetAuthenticatedClientUseCase, GetAuthenticatedClientUseCase>()
            .AddScoped<IGetConfigurationOptionsUseCase, GetConfigurationOptionsUseCase>();

        return services;
    }

    public static IServiceCollection AddCorrelation(this IServiceCollection services)
    {
        services
            .AddSingleton<IExtractCorrelationsFromMetadataUseCase, ExtractCorrelationsFromMetadataUseCase>()
            .AddSingleton<IStringToHashMapper, StringToHashMapper>();

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
            .AddSingleton<IDirectoryInfoFactory, DirectoryInfoFactory>()
            .AddSingleton<IGraphServiceClientFactory, GraphServiceClientFactory>()
            .AddSingleton<IStringBuilderFactory, StringBuilderFactory>()
            .AddSingleton<ITokenCredentialOptionsFactory, TokenCredentialOptionsFactory>()
            .AddSingleton<IUsernamePasswordCredentialFactory, UsernamePasswordCredentialFactory>();

        return services;
    }

    public static IServiceCollection AddForwarders(this IServiceCollection services)
    {
        services
            .AddSingleton<ICultureInfoForwarder, CultureInfoForwarder>()
            .AddSingleton<IDateTimeForwarder, DateTimeForwarder>()
            .AddSingleton<IRegexForwarder, RegexForwarder>()
            .AddSingleton<ISha1Forwarder, Sha1Forwarder>()
            .AddSingleton<IUtf8Forwarder, Utf8Forwarder>();

        return services;
    }
}
