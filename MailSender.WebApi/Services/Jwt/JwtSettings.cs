using System.ComponentModel.DataAnnotations;

namespace MailSender.WebApi.Services.Jwt;

public class JwtSettings
{
    public const string SectionName = "Jwt";
    [Required]
    public required string Key { get; set; }
    [Required]
    public required string Issuer { get; set; }
    [Required]
    public string Audience { get; set; } = string.Empty;
    public int AccessTokenExpirationDays { get; set; } = 90;
}
