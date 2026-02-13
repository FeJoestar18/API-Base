using Microsoft.Extensions.DependencyInjection;

namespace ApiBase.API.Extensions;

public static class CorsExtensions
{
    private const string CorsPolicyName = "DefaultCors";

    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        var corsOrigins = Environment.GetEnvironmentVariable("CORS_ALLOWED_ORIGINS");

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, policy =>
            {
                if (!string.IsNullOrWhiteSpace(corsOrigins))
                {
                    var origins = corsOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
                }
                else
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                }
            });
        });

        return services;
    }

    public static string GetCorsPolicyName() => CorsPolicyName;
}
