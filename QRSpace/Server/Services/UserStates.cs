using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using QRSpace.Server.Entities;
using StackExchange.Redis;

namespace QRSpace.Server.Services
{
    public class UserStates : IUserStates
    {
        private static readonly ConcurrentDictionary<ulong, byte> States = new ConcurrentDictionary<ulong, byte>();
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RedisHelper _redisHelper;
        private readonly IDatabase _state;

        public UserStates(UserManager<ApplicationUser> userManager, RedisHelper redisHelper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _redisHelper = redisHelper ?? throw new ArgumentNullException(nameof(redisHelper));
            _state = redisHelper.GetUserStateDb();
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
        /// <exception cref="ArgumentException"></exception>
        public async Task SetStateAsync(string username, byte userState)
        {
            
            var i = await _userManager.FindByNameAsync(username);
            if (i != null)
            {
                var id =i.Id;
                await _state.HashGetAsync(id.ToString(), new RedisValue(), CommandFlags.None);
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
                throw new ArgumentException(null, nameof(username));
            }
        }

        public async Task RemoveStateAsync(string username, byte userState)
        {
            
        }
    }
}