namespace MailSender.Core.Interfaces;

public interface IMailSenderProvider
{
    Task SendEmailAsync(string to, string subject, string body);
}
