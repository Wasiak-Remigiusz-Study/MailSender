using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MailSender.WebApi.Services.Jwt;

public class JwtTokenService
{
    private readonly IOptionsMonitor<JwtSettings> _options;

    public JwtTokenService(IOptionsMonitor<JwtSettings> options)
    {
        _options = options;
    }

    public string GenerateToken(string appId, string appName)
    {
        var settings = _options.CurrentValue;

        var claims = new[]
        {
            new Claim("appId", appId),
            new Claim("appName", appName)
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));

        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(settings.AccessTokenExpirationDays),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
