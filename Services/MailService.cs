using System.Text;
using System.Text.Json;

namespace MailSender.Services;

public class MailService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public MailService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        subject = ProcessSubject(subject);
        body = ProcessBody(body);

        var apiKey = _configuration["Brevo:ApiKey"];

        var payload = new
        {
            sender = new
            {
                email = _configuration["Brevo:SenderEmail"],
            },
            to = new[]
            {
                new { email = to }
            },
            subject,
            htmlContent = $"<html><body>{body}</body></html>"
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email");
        request.Headers.Add("api-key", apiKey);
        request.Content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
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
