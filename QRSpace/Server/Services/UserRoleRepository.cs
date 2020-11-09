using Microsoft.EntityFrameworkCore;
using QRSpace.Server.Data;
using QRSpace.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRSpace.Server.Services
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRoleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<UserItemDto>> GetUsersWithRolesAsync()
        {
            var result = from userRole in _dbContext.UserRoles
                         join user in _dbContext.Users on userRole.UserId equals user.Id
                         join role in _dbContext.Roles on userRole.RoleId equals role.Id
                         select new UserItemDto { Id = user.Id, Name = user.UserName, Role = role.Name };
            return await result.ToListAsync();
        }

        public async Task<UserItemDto> GetUserWithRolesAsync(ulong userId)
        {
            var result = from userRole in _dbContext.UserRoles
                         join user in _dbContext.Users on userRole.UserId equals user.Id
                         join role in _dbContext.Roles on userRole.RoleId equals role.Id
                         where user.Id == userId
                         select new UserItemDto { Id = user.Id, Name = user.UserName, Role = role.Name };
            return await result.FirstOrDefaultAsync();
        }
    }
}