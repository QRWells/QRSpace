using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRSpace.Server.Entities;
using QRSpace.Server.Services;
using QRSpace.Shared.Models;
using QRSpace.Shared.Models.ActionResults;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QRSpace.Shared.Utils;
using DeleteDto = QRSpace.Shared.Models.LoginDto;

namespace QRSpace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserIdGenerator _generator;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public AccountsController(
            UserManager<ApplicationUser> userManager,
            IUserIdGenerator generator,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _generator = generator ?? throw new ArgumentNullException(nameof(generator));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="registerModel">The model of the new user</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(RegisterResult))]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(RegisterResult))]
        public async Task<ActionResult<RegisterResult>> RegisterAsync([FromForm] RegisterDto registerModel)
        {
            var isExist = await _userManager.FindByNameAsync(registerModel.Username);
            if (isExist !=null)
                return BadRequest(new RegisterResult {Errors = new[] {"Username has been used"}});
            var newUser = new ApplicationUser { Id = _generator.NextId(), UserName = registerModel.Username };
            var password = EncryptHelper.DecryptWithAES(registerModel.ConfirmPassword);
            var result = await _userManager.CreateAsync(newUser, password);
            var result2 = await _userManager.AddToRoleAsync(newUser, "common");
            if (result.Succeeded && result2.Succeeded)
                return Created(HttpContext.Request.GetDisplayUrl(), new RegisterResult { Success = true });
            var errors = result.Errors.Select(x => x.Description);
            return BadRequest(new RegisterResult { Success = false, Errors = errors });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="changePwdDto"></param>
        /// <returns></returns>
        [HttpPut("password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePwdDto changePwdDto)
        {
            var user = await _userManager.FindByNameAsync(changePwdDto.UserName);
            var oldPwd = EncryptHelper.DecryptWithAES(changePwdDto.CurrentPwd);
            var newPwd = EncryptHelper.DecryptWithAES(changePwdDto.NewPwd);
            var result = await _userManager.ChangePasswordAsync(user, oldPwd, newPwd);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _signInManager.SignOutAsync();
            return Ok();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="changeNameDto"></param>
        /// <returns></returns>
        [HttpPut("username")]
        public async Task<IActionResult> ChangeUsernameAsync([FromBody] ChangeNameDto changeNameDto)
        {
            var user = await _userManager.FindByNameAsync(changeNameDto.OldUserName);
            var result = await _userManager.SetUserNameAsync(user, changeNameDto.NewUserName);
            return result.Succeeded ? (IActionResult)Ok() : BadRequest(result.Errors);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteDto deleteDto)
        {
            var user = await _userManager.FindByNameAsync(deleteDto.Username);
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded ? (IActionResult)Ok() : BadRequest(result.Errors);
        }

        // TODO: Forget Password
    }
}