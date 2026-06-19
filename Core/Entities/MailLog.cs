namespace MailSender.Core.Entities;

public class MailLog
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty; // "powodzenie" / "błąd"
    public string? ErrorMessage { get; set; }
    public string Recipient { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int ClientAppId { get; set; }
    public ClientApp ClientApp { get; set; } = null!;
}
