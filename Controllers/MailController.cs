using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MailSender.Models;
using MailSender.Application.Services;

namespace MailSender.Controllers;

[ApiController]
[Route("mail")]
public class MailController : ControllerBase
{
    private readonly MailService _mailService;

    public MailController(MailService mailService)
    {
        _mailService = mailService;
    }

    [Authorize]
    [HttpPost("send")]
    public async Task<IActionResult> Send(MailRequest request)
    {
        var appId = User.FindFirst("appId")?.Value;
        var appName = User.FindFirst("appName")?.Value;

        await _mailService.SendEmailAsync(appId!, request.To, request.Subject, request.Body);

        return Accepted(new
            {
                appId,
                appName,
                status = "queued",
                email = new
                {
                    request.To,
                    subject = request.Subject,
                    body = request.Body
                }
            });
    }
}