using System.Text;
using System.Text.Json;
using MailSender.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MailSender.Infrastructure.Providers;

public class BrevoMailSender : IMailSenderProvider
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public BrevoMailSender(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
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
}
