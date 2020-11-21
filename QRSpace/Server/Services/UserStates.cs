using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using QRSpace.Server.Entities;

namespace QRSpace.Server.Services
{
    public class UserStates : IUserStates
    {
        private static ConcurrentDictionary<ulong, ulong> States { get; } = new ConcurrentDictionary<ulong, ulong>();
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RedisHelper _redisHelper;

        public UserStates(UserManager<ApplicationUser> userManager, RedisHelper redisHelper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _redisHelper = redisHelper ?? throw new ArgumentNullException(nameof(redisHelper));
        }

        public async Task SetLoginAsync(string username) =>
            await SetStateAsync(username, Shared.Enums.UserStates.StateUserLoggedIn);

        public async Task SetLogoutAsync(string username) =>
            await RemoveStateAsync(username, Shared.Enums.UserStates.StateUserLoggedIn);

        public async Task SetUserAwayAsync(string username) =>
            await SetStateAsync(username, Shared.Enums.UserStates.StateUserAway);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userState"><see cref="QRSpace.Shared.Enums.UserStates"/></param>
        public async Task SetStateAsync(string username, ulong userState)
        {
            
            var i = await _userManager.FindByNameAsync(username);
            if (i != null)
            {
                var id =i.Id;
                if (States.TryGetValue(id, out var state))
                {
                    state &= userState;
                    States[id] = state;
                }
                else
                {
                    
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public async Task RemoveStateAsync(string username, ulong userState)
        {
            
        }
    }
}