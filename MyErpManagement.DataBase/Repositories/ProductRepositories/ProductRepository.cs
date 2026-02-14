using Microsoft.EntityFrameworkCore;
using MyErpManagement.Core.Enums;
using MyErpManagement.Core.Modules.ProductsModule.Entities;
using MyErpManagement.Core.Modules.ProductsModule.Enums;
using MyErpManagement.Core.Modules.ProductsModule.IRepositories;
using MyErpManagement.Core.Modules.ProductsModule.Models;
using System.Linq.Expressions;

namespace MyErpManagement.DataBase.Repositories.ProductRepositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IQueryable<TResult>> FindProductsByQuery<TResult>(
            ProductListQueryModel productListQuery,
            Expression<Func<Product, TResult>> selector)
        {
            List<Guid> categoryIds = new List<Guid>();
            if (productListQuery.CategroyId is not null)
            {
                var sql = @"
                WITH RECURSIVE CategoryTree AS (
                    SELECT ""Id"" FROM ""ProductCategories"" 
                    WHERE ""Id"" = @p0
                    UNION ALL
                    SELECT t.""Id"" FROM ""ProductCategories"" t
                    INNER JOIN CategoryTree ct ON t.""ParentId"" = ct.""Id""
                )
                SELECT ""Id"" FROM CategoryTree";

                // 這裡只查 ID，繞過 EF 對實體查詢的包裝限制
                categoryIds = await _db.Database
                    .SqlQueryRaw<Guid>(sql, new Guid(productListQuery.CategroyId))
                    .ToListAsync();
            }

            var query = _db.Products
                .AsNoTracking()
                .Include(p => p.ProductCategory)
                .Where(p => productListQuery.CategroyId == null || categoryIds.Contains(p.ProductCategoryId));
            query = ApplySorting(query, productListQuery);
            return query.Select(selector);
        }

        private static IQueryable<Product> ApplySorting(
            IQueryable<Product> query,
            ProductListQueryModel model)
        {
            var isDesc = model.SortDir == SortDirEnum.desc;

            return model.SortBy switch
            {
                ProductListSortByEnum.name => isDesc
                    ? query.OrderByDescending(p => p.Name)
                    : query.OrderBy(p => p.Name),

                ProductListSortByEnum.salesPrice => isDesc
                    ? query.OrderByDescending(p => p.SalesPrice)
                    : query.OrderBy(p => p.SalesPrice),

                ProductListSortByEnum.purchasePrice => isDesc
                    ? query.OrderByDescending(p => p.PurchasePrice)
                    : query.OrderBy(p => p.PurchasePrice),

                ProductListSortByEnum.createdAt => isDesc
                    ? query.OrderByDescending(p => p.CreatedAt)
                    : query.OrderBy(p => p.CreatedAt),

                _ => query.OrderByDescending(p => p.CreatedAt) // 預設排序
            };
        }
    }
}
