using MyErpManagement.Core.Modules.ProductsModule.Entities;
using MyErpManagement.Core.Modules.ProductsModule.IRepositories;

namespace MyErpManagement.DataBase.Repositories
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
