using MyErpManagement.Core.Modules.ProductsModule.Entities;

namespace MyErpManagement.DataBase.SeedData
{
    public class ProductCategorySeedData
    {
        public ProductCategory ProductCategory { get; set; }
        public ProductCategorySeedData()
        {
            ProductCategory = new ProductCategory {
                Id = Guid.Parse("F21535A6-A2C1-B5D2-92DE-B14732913BA7"),
                Name = "全部",
                CreatedAt = new DateTime(2025, 12, 12), // 建議用固定日期，DateTime.Now 會導致每次 migration 都產生變化
                SortOrder = 0,
                ParentId = null
            };
        }
    }
}
