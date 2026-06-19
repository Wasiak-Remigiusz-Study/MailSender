using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MailSender.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MailSender.Controllers;

[ApiController]
[Route("mail-log")]
[Authorize]
public class MailLogController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public MailLogController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var appId = User.FindFirst("appId")?.Value;

        var logs = await _dbContext.MailLogs
            .Include(m => m.ClientApp)
            .Where(m => m.ClientApp.AppId == appId)
            .Select(m => new
            {
                m.Id,
                m.Status,
                m.Recipient,
                m.Subject,
                m.ErrorMessage,
                m.CreatedAt
            })
            .ToListAsync();

        return Ok(logs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var appId = User.FindFirst("appId")?.Value;

        var log = await _dbContext.MailLogs
            .Include(m => m.ClientApp)
            .FirstOrDefaultAsync(m => m.Id == id && m.ClientApp.AppId == appId);

        if (log == null)
        {
            return NotFound(new { error = "Log not found or you do not have permission to view it." });
        }

        return Ok(new
        {
            log.Id,
            log.Status,
            log.Recipient,
            log.Subject,
            log.ErrorMessage,
            log.CreatedAt
        });
    }
}
