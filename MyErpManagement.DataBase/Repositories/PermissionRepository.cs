using Microsoft.EntityFrameworkCore;
using MyErpManagement.Core.Modules.UsersModule.Entities;
using MyErpManagement.Core.Modules.UsersModule.IRepositories;

namespace MyErpManagement.DataBase.Repositories
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        private ApplicationDbContext _db;
        public PermissionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<Permission>> GetAllAsync()
        {
            return await _db.Permissions.ToListAsync();
        }

        public void AddRange(IEnumerable<Permission> permissions)
        {
            _db.Permissions.AddRange(permissions);
        }
    }
}
