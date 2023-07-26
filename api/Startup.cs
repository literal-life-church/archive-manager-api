using LiteralLifeChurch.ArchiveManagerApi.Authentication.Domain.Model;
using LiteralLifeChurch.ArchiveManagerApi.DI.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(LiteralLifeChurch.ArchiveManagerApi.Startup))]
namespace LiteralLifeChurch.ArchiveManagerApi
{
    internal class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            builder.ConfigurationBuilder
                .SetBasePath(context.ApplicationRootPath)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder
                .Services
                .AddOptions<AuthenticationEnvironmentVariableDomainModel>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("ArchiveManagerApi").Bind(settings);
                });

            builder.Services.AddAuthentication();
        }
    }
}
