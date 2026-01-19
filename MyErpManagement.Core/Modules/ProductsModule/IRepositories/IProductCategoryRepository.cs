using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.ProductsModule.Entities;
using MyErpManagement.Core.Modules.ProductsModule.Models;

namespace MyErpManagement.Core.Modules.ProductsModule.IRepositories
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        Task<List<ProductCategoryTreeModel>> GetCategoryTreeAsync();
    }
}
