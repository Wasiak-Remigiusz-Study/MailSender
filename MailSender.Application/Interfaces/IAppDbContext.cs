using MailSender.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MailSender.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<ClientApp> ClientApps { get; }
    DbSet<MailLog> MailLogs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
