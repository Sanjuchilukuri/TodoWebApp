using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Service.Interfaces;

namespace ToDo. WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        public TokenController(IConfiguration config, ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;
        }

        [HttpGet("{refreshToken}")]
        public IActionResult GetToken([FromRoute] string refreshToken)
        {
            if (_tokenService.ValidateRefreshToken(refreshToken))
            {
                return Ok(_tokenService.GenerateNewTokens(refreshToken));
            }
            else
            {
                return Unauthorized("Invalid token");
            }
        }
    }
}