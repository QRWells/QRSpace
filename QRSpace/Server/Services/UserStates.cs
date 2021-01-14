using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using QRSpace.Server.Entities;
using StackExchange.Redis;

namespace QRSpace.Server.Services
{
    public class UserStates : IUserStates
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDatabase _redisDb;

        public UserStates(UserManager<ApplicationUser> userManager, RedisHelper redisHelper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _redisDb = redisHelper?.GetDatabase() ?? throw new ArgumentNullException(nameof(redisHelper));
        }

        public async Task SetLoginAsync(string username) =>
            await SetStateAsync(username, Shared.Enums.UserStates.Online);

        public async Task SetLogoutAsync(string username) =>
            await RemoveStateAsync(username, Shared.Enums.UserStates.Online);

        public async Task SetUserAwayAsync(string username) =>
            await SetStateAsync(username, Shared.Enums.UserStates.Away);

        /// <summary>
        /// Set the specific <see cref="Shared.Enums.UserStates"/> to specific user.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="userState"><see cref="Shared.Enums.UserStates"/></param>
        private async Task SetStateAsync(string username, Shared.Enums.UserStates userState)
        {
            var i = await _userManager.FindByNameAsync(username);
            if (i != null)
            {
            }
            else
            {
                throw new ArgumentException();
            }
        }

        private async Task RemoveStateAsync(string username, Shared.Enums.UserStates userState)
        {
            var i = await _redisDb.StringGetAsync("");
        }
    }
}