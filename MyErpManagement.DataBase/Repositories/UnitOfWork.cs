using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.CustomerModule.IRepositories;
using MyErpManagement.Core.Modules.JwtModule.IRepositories;
using MyErpManagement.Core.Modules.ProductsModule.IRepositories;
using MyErpManagement.Core.Modules.UsersModule.IRepositories;

namespace MyErpManagement.DataBase.Repositories
{
    public class UnitOfWork(
        ApplicationDbContext db,
        IUserRepository userRepository,
        IPermissionRepository permissionRepository,
        IJwtRecordRepository jwtRecordRepository,
        IProductCategoryRepository productCategoryRepository,
        IProductRepository productRepository,
        ICustomerRepository customerRepository,
        ICustomerTagRepository customerTagRepository
        ) : IUnitOfWork
    {
        public IUserRepository UserRepository => userRepository;
        public IPermissionRepository PermissionRepository => permissionRepository;

        public IJwtRecordRepository JwtRecordRepository => jwtRecordRepository;

        public IProductCategoryRepository ProductCategoryRepository => productCategoryRepository;

        public IProductRepository ProductRepository => productRepository;
        public ICustomerRepository CustomerRepository => customerRepository;
        public ICustomerTagRepository CustomerTagRepository => customerTagRepository;

        private IDbContextTransaction? _currentTransaction;

        public void Save()
        {
            db.SaveChanges();
        }

        public async Task<bool> Complete()
        {
            return await db.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return db.ChangeTracker.HasChanges();
        }

        #region Transcation

        public async Task BeginTransactionAsync()
        {
            if (db.Database.IsInMemory())
            {
                return;
            }
            _currentTransaction = await db.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await db.SaveChangesAsync();
                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        // Example
        //public async Task ProcessOrderAsync()
        //{
        //    await _unitOfWork.BeginTransactionAsync();
        //    try
        //    {
        //        // 操作 1: 透過 Repository 新增資料
        //        await _unitOfWork.UserRepository.AddAsync(newUser);

        //        // 操作 2: 執行你的 ExecuteSqlCommand (Raw SQL)
        //        await _unitOfWork.ExecuteSqlCommand("UPDATE Inventory SET Stock = Stock - 1 WHERE Id = {0}", productId);

        //        // 統一提交
        //        await _unitOfWork.CommitAsync();
        //    }
        //    catch (Exception)
        //    {
        //        // 發生錯誤，全部撤銷
        //        await _unitOfWork.RollbackAsync();
        //        throw;
        //    }
        //}

        #endregion
    }
}
