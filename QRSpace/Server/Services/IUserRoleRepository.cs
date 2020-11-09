using QRSpace.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRSpace.Server.Services
{
    public interface IUserRoleRepository
    {
        public Task<IEnumerable<UserItemDto>> GetUsersWithRolesAsync();

        public Task<UserItemDto> GetUserWithRolesAsync(ulong userId);
    }
}