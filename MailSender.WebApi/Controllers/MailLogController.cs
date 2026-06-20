using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MailSender.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MailSender.WebApi.Controllers;

[ApiController]
[Route("mail-log")]
[Authorize]
public class MailLogController : ControllerBase
{
    private readonly IAppDbContext _dbContext;

    public MailLogController(IAppDbContext dbContext)
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
                AppId = m.ClientApp.AppId,
                AppName = m.ClientApp.AppName,
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
            AppId = log.ClientApp.AppId,
            AppName = log.ClientApp.AppName,
            log.Status,
            log.Recipient,
            log.Subject,
            log.ErrorMessage,
            log.CreatedAt
        });
    }
}
