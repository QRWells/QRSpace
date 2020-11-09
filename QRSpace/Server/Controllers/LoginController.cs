using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QRSpace.Server.Entities;
using QRSpace.Shared.Models;
using QRSpace.Shared.Models.ActionResults;
using System;
using System.Linq;
using System.Threading.Tasks;
using QRSpace.Shared.Utils;
using Microsoft.Extensions.Logging;
using QRSpace.Server.Services;

namespace QRSpace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginController> _logger;
        private readonly IUserStates _userStates;

        public LoginController(IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<LoginController> logger,
            IUserStates userStates)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userStates = userStates ?? throw new ArgumentNullException(nameof(userStates));
        }

        /// <summary>
        /// Login and issue jwt token to client.
        /// Add the current <see cref="ApplicationUser"/> to its role*
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, StatusCode = StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, StatusCode = StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResult>> Login([FromForm] LoginDto loginModel)
        {
            _logger.Log(LogLevel.Warning, "Received login request");
            var password = EncryptHelper.DecryptWithAES(loginModel.Password);
            var result = await _signInManager
                .PasswordSignInAsync(loginModel.Username, password, loginModel.RememberMe, false);
            if (!result.Succeeded)
                return Unauthorized(new LoginResult { Success = false, Error = "Failed to login" });
            
            var user = await _userManager.FindByNameAsync(loginModel.Username);
            var roles = await _userManager.GetRolesAsync(user);
            var config = _configuration.GetSection("JwtConfig");

            await _userStates.SetLoginAsync(loginModel.Username);
            
            var token = JwtTokenHelper.IssueJwtToken(user.UserName, roles.ToList(), config);
            _logger.LogInformation("Login successful");
            return Ok(new LoginResult { Success = true, Token = token });
        }
    }
}