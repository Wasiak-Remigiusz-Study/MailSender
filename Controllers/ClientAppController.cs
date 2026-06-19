using Microsoft.AspNetCore.Mvc;
using MailSender.Models;
using MailSender.Services.Jwt;
using MailSender.Application.Services;

namespace MailSender.Controllers;

[ApiController]
[Route("client-app")]
public class ClientAppController : ControllerBase
{
    private readonly ClientAppService _clientAppService;
    private readonly JwtTokenService _jwtTokenService;

    public ClientAppController(
        ClientAppService clientAppService,
        JwtTokenService jwtTokenService)
    {
        _clientAppService = clientAppService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterClientRequest request)
    {
        var validation = _clientAppService.ValidatePassword(request.Pass);
        if (!validation.IsValid)
        {
            return StatusCode(403, new
            {
                error = $"Invalid index-based password {validation.IndexSuffix}"
            });
        }

        var regResult = await _clientAppService.RegisterAppAsync(request.AppId, request.AppName);
        if (!regResult.IsSuccess)
        {
            return Conflict(new
            {
                error = $"client app duplication. Exisiting {regResult.AppId} {regResult.AppName}"
            });
        }

        var token = _jwtTokenService.GenerateToken(request.AppId, request.AppName);
        return Ok(new RegisterClientResponse
        {
            AppId = request.AppId,
            AppName = request.AppName,
            Key = token
        });
    }
}
