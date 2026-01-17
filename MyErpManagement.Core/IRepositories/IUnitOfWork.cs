using Microsoft.Data.SqlClient;
using MyErpManagement.Core.Modules.CustomerModule.IRepositories;
using MyErpManagement.Core.Modules.JwtModule.IRepositories;
using MyErpManagement.Core.Modules.ProductsModule.IRepositories;
using MyErpManagement.Core.Modules.UsersModule.IRepositories;

namespace MyErpManagement.Core.IRepositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IJwtRecordRepository JwtRecordRepository { get; }
        IProductCategoryRepository ProductCategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        ICustomerTagRepository CustomerTagRepository { get; }
        int ExecuteSqlCommand(string sql, params SqlParameter[] sqlParameters);

        public void Save();
        public Task<bool> Complete();
        public bool HasChanges();

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
