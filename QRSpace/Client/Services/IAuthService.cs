using QRSpace.Shared.Models;
using QRSpace.Shared.Models.ActionResults;
using System.Threading.Tasks;

namespace QRSpace.Client.Services
{
    public interface IAuthService
    {
        public Task<RegisterResult> RegisterAsync(RegisterDto registerModel);

        public Task<LoginResult> LoginAsync(LoginDto loginModel);

        public Task LogoutAsync();
    }
}