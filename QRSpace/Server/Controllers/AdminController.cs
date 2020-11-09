using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRSpace.Server.Entities;
using QRSpace.Server.Services;
using QRSpace.Shared.Models;

namespace QRSpace.Server.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserIdGenerator _generator;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRoleRepository _userRoleRepository;

        public AdminController(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IUserRoleRepository userRoleRepository,
            IUserIdGenerator generator)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _userRoleRepository =
                userRoleRepository ?? throw new ArgumentNullException(nameof(userRoleRepository));
            _generator = generator ?? throw new ArgumentNullException(nameof(generator));
        }

        /// <summary>
        ///     Get all users
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserItemDto>>> GetUsers()
        {
            var users = await _userRoleRepository.GetUsersWithRolesAsync();
            return Ok(users);
        }

        /// <summary>
        ///     Get a specific user by id.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns></returns>
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUser([FromRoute] ulong userId)
        {
            var user = await _userRoleRepository.GetUserWithRolesAsync(userId);
            return Ok(user);
        }

        /// <summary>
        ///     Get a specific user by name.
        /// </summary>
        /// <param name="userName">The name of the user</param>
        /// <returns></returns>
        [HttpGet("users/{userName}")]
        public async Task<IActionResult> GetUser([FromRoute] string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return Ok(user);
        }

        /// <summary>
        ///     Add a new user
        /// </summary>
        /// <param name="create">The information of the user to add</param>
        /// <returns></returns>
        [HttpPost("users")]
        public async Task<IActionResult> AddUser([FromForm] CreateDto create)
        {
            var newUser = new ApplicationUser {UserName = create.UserName, Id = _generator.NextId()};
            var result = await _userManager.CreateAsync(newUser, create.Password);
            var result2 = await _userManager.AddToRoleAsync(newUser, create.Role);
            if (result.Succeeded && result2.Succeeded)
                return Created(HttpContext.Request.GetDisplayUrl(), new {Success = true});

            return BadRequest(result.Errors);
        }

        /// <summary>
        ///     Update a specific user.
        /// </summary>
        /// <param name="userId">The id of the user to update</param>
        /// <param name="update">The content to update</param>
        /// <returns></returns>
        [HttpPut("users/{userId}")]
        public async Task<IActionResult> UpdateUser([FromRoute] ulong userId, [FromForm] UpdateDto update)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (update.Role != "none") await _userManager.AddToRoleAsync(user, update.Role);

            if (string.IsNullOrEmpty(update.Name)) return StatusCode(500);
            user.UserName = update.Name;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded ? (IActionResult) Ok() : BadRequest(result.Errors);
        }

        /// <summary>
        ///      Delete a specific user by its id.
        /// </summary>
        /// <param name="userId">The id of the user to delete</param>
        /// <returns></returns>
        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser([FromRoute] ulong userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return StatusCode(500, result.Errors.Select(e => e.Description));

            return NoContent();
        }

        /// <summary>
        ///      Add a new role.
        /// </summary>
        /// <param name="roleName">The name of the new role</param>
        /// <returns></returns>
        [HttpPost("roles")]
        public async Task<IActionResult> AddRole([FromBody] string roleName)
        {
            var result = await _roleManager.CreateAsync(new ApplicationRole {Name = roleName, Id = 1});
            return result.Succeeded ? StatusCode(500) : Ok();
        }

        /// <summary>
        ///     Get all the roles.
        /// </summary>
        /// <returns>Ok if </returns>
        [HttpGet("roles")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IActionResult))]
        public IActionResult GetRoles()
        {
            return Ok(_roleManager.Roles.Select(r => new {r.Name, r.Id}));
        }
        
        /// <summary>
        /// Reset the password of the specific user.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="newPassword">The new password to change</param>
        /// <returns></returns>
        [HttpPut("users/{userId}")]
        public async Task<IActionResult> ResetPassword(ulong userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.IsCompletedSuccessfully)
            {
                return Ok();
            }

            return BadRequest(result.Exception?.Message);
        }
    }
}