using System.Text;
using System.Text.Json;
using MailSender.Core.Interfaces;

namespace MailSender.Infrastructure.Providers;

public class MailTrapMailSender : IMailSenderProvider
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public MailTrapMailSender(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var apiKey = _configuration["MailTrap:ApiKey"] ?? "dummy-key";

        var payload = new
        {
            from = new
            {
                email = "hello@example.com",
                name = "Mailtrap Test"
            },
            to = new[]
            {
                new { email = to }
            },
            subject,
            text = body
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "https://send.api.mailtrap.io/api/send");
        request.Headers.Add("Api-Token", apiKey);
        request.Content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"MailTrap Error: {content}");
        }
    }
}
