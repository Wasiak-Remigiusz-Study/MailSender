using MailSender.Core.Interfaces;

namespace MailSender.Application.Services;

public class MailService
{
    private readonly IConfiguration _configuration;
    private readonly IMailSenderProvider _mailSenderProvider;

    public MailService(IConfiguration configuration, IMailSenderProvider mailSenderProvider)
    {
        _configuration = configuration;
        _mailSenderProvider = mailSenderProvider;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        subject = ProcessSubject(subject);
        body = ProcessBody(body);

        await _mailSenderProvider.SendEmailAsync(to, subject, body);
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
