using Microsoft.Extensions.DependencyInjection;

namespace ApiBase.Infrastructure.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            return services;
        }
    }
}