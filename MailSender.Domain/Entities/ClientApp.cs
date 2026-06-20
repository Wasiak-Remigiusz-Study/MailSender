namespace MailSender.Domain.Entities;

public class ClientApp
{
    public int Id { get; set; }
    public string AppId { get; set; } = string.Empty;
    public string AppName { get; set; } = string.Empty;

    public List<MailLog> MailLogs { get; set; } = new();
}
