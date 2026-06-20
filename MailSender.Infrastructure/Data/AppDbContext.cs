using MailSender.Domain.Entities;
using MailSender.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MailSender.Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ClientApp> ClientApps { get; set; } = null!;
    public DbSet<MailLog> MailLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ClientApp>()
            .HasIndex(c => c.AppId)
            .IsUnique();
            
        modelBuilder.Entity<MailLog>()
            .HasOne(m => m.ClientApp)
            .WithMany(c => c.MailLogs)
            .HasForeignKey(m => m.ClientAppId);
    }
}
