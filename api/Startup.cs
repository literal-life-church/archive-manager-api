using LiteralLifeChurch.ArchiveManagerApi;
using LiteralLifeChurch.ArchiveManagerApi.DI.Extensions;
using LiteralLifeChurch.ArchiveManagerApi.Global.Domain.Model;
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
            .AddLogging()
            .AddOptions<AuthenticationEnvironmentVariableDomainModel>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("ArchiveManagerApi").Bind(settings);
            });

        builder
            .Services
            .AddAuthentication()
            .AddDrive()
            .AddExtraction()
            .AddFactories()
            .AddForwarders();
    }
}
