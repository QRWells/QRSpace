using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRSpace.Server.Entities;
using System;
using System.Threading.Tasks;

namespace QRSpace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LogoutController(
            SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        [HttpPost]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}