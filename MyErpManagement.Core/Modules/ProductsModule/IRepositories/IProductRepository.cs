using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.ProductsModule.Entities;
using MyErpManagement.Core.Modules.ProductsModule.Models;
using System.Linq.Expressions;

namespace MyErpManagement.Core.Modules.ProductsModule.IRepositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IQueryable<TResult>> FindProductsByQuery<TResult>(ProductListQueryModel Dto, Expression<Func<Product, TResult>> selector);
    }
}
