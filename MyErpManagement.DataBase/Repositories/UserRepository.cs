using Microsoft.EntityFrameworkCore;
using MyErpManagement.Core.Modules.UsersModule.Entities;
using MyErpManagement.Core.Modules.UsersModule.IRepositories;

namespace MyErpManagement.DataBase.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<User?> FindUserByIdAsync(string id)
        {
            var userGuId = new Guid(id);
            return await _db.Users.FindAsync(userGuId);
        }

        public async Task<User?> GetUserByUserAccountAsync(string account)
        {
            return await _db.Users
            .SingleOrDefaultAsync(x => x.Account == account);
        }

        public async Task<List<string>> FindUserPermissionsAsync(string userId)
        {
            var userGuId = new Guid(userId);
            // 從 User 開始，一路 Include 到最底層的 Permission
            var permissions = await _db.Users
                .Where(u => u.Id == userGuId)
                .SelectMany(u => u.UserRoles)             // 進入 UserRole
                .Select(ur => ur.Role)                    // 進入 Role
                .SelectMany(r => r.RolePermissions)       // 進入 RolePermission
                .Select(rp => rp.Permission.PermissionKey) // 最終取得 PermissionKey 字串
                .Distinct()                               // 去除重複 (如果不同角色有相同權限)
                .ToListAsync();

            return permissions;
        }
    }
}
