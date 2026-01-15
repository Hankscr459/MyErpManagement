using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.UsersModule.Entities;

namespace MyErpManagement.Core.Modules.UsersModule.IRepositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<List<Permission>> GetAllAsync();
        void AddRange(IEnumerable<Permission> permissions);
    }
}
