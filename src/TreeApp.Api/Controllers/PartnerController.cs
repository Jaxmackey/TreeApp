using Microsoft.AspNetCore.Mvc;
using TreeApp.Application.DTOs;

namespace TreeApp.Api.Controllers;

[ApiController]
public class PartnerController : ControllerBase
{
    [HttpPost("api.user.partner.rememberMe")]
    public ActionResult<TokenInfo> RememberMe([FromQuery] string code)
    {
        // Для демо: генерируем dummy JWT (без реальной безопасности)
        var token = "dummy_jwt_token_for_" + code;
        return Ok(new TokenInfo { Token = token });
    }
}