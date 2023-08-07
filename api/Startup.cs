using LiteralLifeChurch.ArchiveManagerApi;
using LiteralLifeChurch.ArchiveManagerApi.Config.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.DI.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace LiteralLifeChurch.ArchiveManagerApi;

internal class Startup : FunctionsStartup
{
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        var context = builder.GetContext();

        builder.ConfigurationBuilder
            .SetBasePath(context.ApplicationRootPath)
            .AddJsonFile("local.settings.json", true, true)
            .AddJsonFile("settings.json", true, true)
            .AddEnvironmentVariables();
    }

    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder
            .Services
            .AddOptions<AuthenticationOptionsDomainModel>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration
                    .GetSection("ArchiveManagerApi:Auth")
                    .Bind(settings);
            });

        builder
            .Services
            .AddOptions<ConfigurationOptionsDomainModel>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration
                    .GetSection("ArchiveManagerApi:Options")
                    .Bind(settings);
            });

        builder
            .Services
            .AddConfiguration()
            .AddCorrelation()
            .AddDrive()
            .AddExtraction()
            .AddFactories()
            .AddForwarders()
            .AddLogging();
    }
}
