using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace QRSpace.Server.Services
{
    public interface IUserStates
    {
        public Task SetLoginAsync(string username);

        public Task SetLogoutAsync(string username);

        public Task SetUserAwayAsync(string username);

        public Task SetStateAsync(string username, ulong userState);

        public Task RemoveStateAsync(string username, ulong userState);
    }
}