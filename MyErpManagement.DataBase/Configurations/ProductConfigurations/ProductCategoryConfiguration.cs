using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.ProductsModule.Entities;
using MyErpManagement.DataBase.SeedData;

namespace MyErpManagement.DataBase.Configurations.ProductConfigurations
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        private readonly ProductCategorySeedData _productCategorySeedData;

        public ProductCategoryConfiguration()
        {
            _productCategorySeedData = new ProductCategorySeedData();
        }

        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.Property(x => x.Id)
                .HasDefaultValueSql("gen_random_uuid()"); // 讓資料庫產生有序的 Guid
            // 為 parentId 建立索引，加速 CTE 遞迴尋找子層
            builder.HasIndex(gt => gt.ParentId).HasDatabaseName("IX_productCategory_parentId");

            builder.HasData(_productCategorySeedData.ProductCategory);
            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("now()");
            builder.HasOne(x => x.Parent)
               .WithMany(x => x.Children)
               .HasForeignKey(x => x.ParentId)
               .OnDelete(DeleteBehavior.Restrict); // 避免循環刪除錯誤

            builder.HasIndex(pc => pc.Name).IsUnique();
        }
    }
}
