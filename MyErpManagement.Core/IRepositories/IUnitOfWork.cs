using MyErpManagement.Core.Modules.CustomerModule.IRepositories;
using MyErpManagement.Core.Modules.JwtModule.IRepositories;
using MyErpManagement.Core.Modules.OrderNoModule.IRepositories;
using MyErpManagement.Core.Modules.ProductsModule.IRepositories;
using MyErpManagement.Core.Modules.SupplierModule.IRepositories;
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
        ISupplierRepository SupplierRepository { get; }
        ISupplierTagRepository SupplierTagRepository { get; }
        IOrderSequenceRepository OrderSequenceRepository { get; }

        public void Save();
        public Task<bool> Complete();
        public bool HasChanges();

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
