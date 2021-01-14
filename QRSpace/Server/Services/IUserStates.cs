using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace QRSpace.Server.Services
{
    public interface IUserStates
    {
        public Task SetLoginAsync(string username);

        public Task SetLogoutAsync(string username);

        public Task SetUserAwayAsync(string username);
    }
}