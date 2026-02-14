using Microsoft.EntityFrameworkCore;
using MyErpManagement.Core.Modules.ProductsModule.Entities;
using MyErpManagement.Core.Modules.ProductsModule.IRepositories;
using MyErpManagement.Core.Modules.ProductsModule.Models;

namespace MyErpManagement.DataBase.Repositories.ProductRepositories
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        private ApplicationDbContext _db;
        public ProductCategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<List<ProductCategoryTreeModel>> GetCategoryTreeAsync()
        {
            // 1. 從資料庫取得所有類別（不分層級）
            var allCategories = await _db.ProductCategories
                .AsNoTracking()
                .OrderBy(c => c.SortOrder)
                .Select(c => new ProductCategoryTreeModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ParentId = c.ParentId,
                    SortOrder = c.SortOrder
                })
                .ToListAsync();

            // 2. 使用 Dictionary 加速尋找對象
            var categoryDict = allCategories.ToDictionary(c => c.Id);
            var rootNodes = new List<ProductCategoryTreeModel>();

            // 3. 走訪所有節點，將其掛載到對應的父節點下
            foreach (var category in allCategories)
            {
                if (category.ParentId == null || !categoryDict.ContainsKey(category.ParentId.Value))
                {
                    // 沒有父 ID，或是找不到父 ID 的，視為第一層（根節點）
                    rootNodes.Add(category);
                }
                else
                {
                    // 找到父節點，並將自己加入父節點的 Children 列表中
                    categoryDict[category.ParentId.Value].Children.Add(category);
                }
            }

            return rootNodes;
        }
    }
}
