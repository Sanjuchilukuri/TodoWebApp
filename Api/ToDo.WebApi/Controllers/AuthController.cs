using ToDo.Service.DTO.User;
using ToDo.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ToDo.WebApi.Controllers
{
    /// <summary>
    /// Controller for managing user authentication
    /// </summary>
    [ApiController]
    [Route("[controller]/[Action]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IConfiguration _config;

        private readonly ITokenService _tokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="userServices"><see cref="IUser"/> </param>
        /// <param name="config"><see cref="IConfiguration"/></param>
        public AuthController(IUserService userService, IConfiguration config, ITokenService tokenService)
        {
            _userService = userService;
            _config = config;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="login">The user details to login</param>
        /// <returns> The JWT token for the logged in user.</returns>
        /// <exception cref="Exception">Thrown when the user credentials are invalid.</exception>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRegisterModel login)
        {
            var loggedInUser = await _userService.Login(login);

            if (loggedInUser != null)
            {
                var jwttoken = _tokenService.GenerateAccessToken(loggedInUser.Id);
                var refreshToken = _tokenService.GenerateRefreshToken(loggedInUser.Id);
                var response = new { jwttoken, refreshToken };
                return Ok(response);
            }
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">The new user to register.</param>
        /// <returns>The registered User.</returns>
        /// <exception cref="Exception">Thrown when the user could not be added.</exception>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] LoginRegisterModel user)
        {
            if (await _userService.IsUserExisted(user.UserName))
            {
                return BadRequest("User already exists");
            }

            if (await _userService.Register(user))
            {
                return Created(nameof(Register), user.UserName);
            }

            throw new Exception("Failed to Register User");
        }
    }
}
