using MailSender.WebApi.Services.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MailSender.WebApi.Services;

public static class AuthenticationSetupExtensions
{
    public static IServiceCollection AddJwtTokenService(this IServiceCollection services, IConfigurationSection jwtSection)
    {
        services.AddOptions<JwtSettings>()
            .Bind(jwtSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddScoped<JwtTokenService>();
        return services;
    }
}
