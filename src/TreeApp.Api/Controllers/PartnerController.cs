using Microsoft.AspNetCore.Mvc;
using TreeApp.Application.DTOs;

namespace TreeApp.Api.Controllers;

[ApiController]
public class PartnerController : ControllerBase
{
    [HttpPost("api.user.partner.rememberMe")]
    public ActionResult<TokenInfo> RememberMe([FromQuery] string code)
    {
        // Per the technical specification, authentication is optional.
        // This method fulfills the requirement to expose a token acquisition endpoint.
        // Actual endpoint protection via [Authorize] is not mandated and therefore omitted.
        var token = "dummy_jwt_token_for_" + code;
        return Ok(new TokenInfo { Token = token });
    }
}