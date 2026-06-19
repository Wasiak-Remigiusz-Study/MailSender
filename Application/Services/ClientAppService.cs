namespace MailSender.Application.Services;
public class ClientAppService
{
    private readonly IConfiguration _configuration;
    public ClientAppService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public PasswordValidationResult ValidatePassword(string pass)
    {
        var indexNumber = _configuration["Student:IndexNumber"] ?? string.Empty;
        var indexSuffix = indexNumber[^2..];
        var expectedPassword = $"q##wa{indexSuffix}";
        if (pass != expectedPassword)
        {
            return new PasswordValidationResult(false, indexSuffix);
        }
        return new PasswordValidationResult(true, indexSuffix);
    }
}
public record PasswordValidationResult(bool IsValid, string IndexSuffix);