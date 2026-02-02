using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.ProductsModule.Entities;

namespace MyErpManagement.DataBase.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasIndex(p => p.ProductCategoryId).HasDatabaseName("IX_products_productCategoryId");
            builder.Property(p => p.Id)
                    .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("now()");
            // 配置索引
            builder.HasIndex(p => p.Code).IsUnique();
            builder.HasIndex(p => p.Specification).IsUnique();
        }
    }
}
