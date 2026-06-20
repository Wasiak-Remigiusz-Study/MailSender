using MailSender.Domain.Entities;
using MailSender.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MailSender.Application.Services;

public class ClientAppService
{
    private readonly IConfiguration _configuration;
    private readonly IAppDbContext _dbContext;

    public ClientAppService(IConfiguration configuration, IAppDbContext dbContext)
    {
        _configuration = configuration;
        _dbContext = dbContext;
    }

    public PasswordValidationResult ValidatePassword(string pass)
    {
        var indexNumber = _configuration["Student:IndexNumber"] ?? string.Empty;
        var indexSuffix = indexNumber.Length >= 2 ? indexNumber[^2..] : indexNumber;
        var expectedPassword = $"q##waQ{indexSuffix}";
        
        if (pass != expectedPassword)
        {
            return new PasswordValidationResult(false, indexSuffix);
        }
        return new PasswordValidationResult(true, indexSuffix);
    }

    public async Task<RegistrationResult> RegisterAppAsync(string appId, string appName)
    {
        var existing = await _dbContext.ClientApps
            .FirstOrDefaultAsync(c => c.AppId == appId || c.AppName == appName);

        if (existing != null)
        {
            return new RegistrationResult(false, existing.AppId, existing.AppName);
        }

        var newApp = new ClientApp
        {
            AppId = appId,
            AppName = appName
        };

        _dbContext.ClientApps.Add(newApp);
        await _dbContext.SaveChangesAsync();

        return new RegistrationResult(true, appId, appName);
    }
}

public record PasswordValidationResult(bool IsValid, string IndexSuffix);
public record RegistrationResult(bool IsSuccess, string AppId, string AppName);
