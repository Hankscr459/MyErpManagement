using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.UsersModule.Entities;

namespace MyErpManagement.Core.Modules.UsersModule.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> FindUserByIdAsync(string id);
        Task<List<string>> FindUserPermissionsAsync(string userId);
    }
}
