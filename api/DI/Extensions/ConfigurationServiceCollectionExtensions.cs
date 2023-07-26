using LiteralLifeChurch.ArchiveManagerApi.Authentication.Domain.UseCase;
using Microsoft.Extensions.DependencyInjection;

namespace LiteralLifeChurch.ArchiveManagerApi.DI.Extensions
{
    internal static class ConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services)
        {
            services
                .AddSingleton<IGetAuthenticatedClientUseCase, GetAuthenticatedClientUseCase>();

            return services;
        }
    }
}
