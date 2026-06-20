namespace MailSender.Application.Models;

public class RegisterClientRequest
{
    public string AppId { get; set; } = string.Empty;

    public string AppName { get; set; } = string.Empty;

    public string Pass { get; set; } = string.Empty;
}
