using MailSender.Core.Interfaces;
using MailSender.Core.Entities;
using MailSender.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MailSender.Application.Services;

public class MailService
{
    private readonly IConfiguration _configuration;
    private readonly IMailSenderProvider _mailSenderProvider;
    private readonly AppDbContext _dbContext;

    public MailService(IConfiguration configuration, IMailSenderProvider mailSenderProvider, AppDbContext dbContext)
    {
        _configuration = configuration;
        _mailSenderProvider = mailSenderProvider;
        _dbContext = dbContext;
    }

    public async Task SendEmailAsync(string appId, string to, string subject, string body)
    {
        subject = ProcessSubject(subject);
        body = ProcessBody(body);

        var clientApp = await _dbContext.ClientApps.FirstOrDefaultAsync(c => c.AppId == appId);
        if (clientApp == null)
        {
            throw new Exception("Application not found in database.");
        }

        var mailLog = new MailLog
        {
            ClientAppId = clientApp.Id,
            Recipient = to,
            Subject = subject
        };

        try
        {
            await _mailSenderProvider.SendEmailAsync(to, subject, body);
            mailLog.Status = "powodzenie";
        }
        catch (Exception ex)
        {
            mailLog.Status = "błąd";
            mailLog.ErrorMessage = ex.Message;
            throw;
        }
        finally
        {
            _dbContext.MailLogs.Add(mailLog);
            await _dbContext.SaveChangesAsync();
        }
    }

    private string ProcessSubject(string subject)
    {
        if (subject.EndsWith('?'))
        {
            return $"[Q]{subject}";
        }

        return subject;
    }

    private string ProcessBody(string body)
    {
        var surname = _configuration["Student:Surname"];
        if (string.IsNullOrWhiteSpace(surname))
        {
            return body;
        }

        if (!body.Contains(surname))
        {
            return body;
        }

        return body.Replace(surname, $"[student.surname]{surname}[/student.surname]");
    }
}
