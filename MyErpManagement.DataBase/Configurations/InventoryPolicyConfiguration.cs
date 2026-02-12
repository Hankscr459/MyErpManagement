using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.InventoryModule.Entities;

namespace MyErpManagement.DataBase.Configurations
{
    public class InventoryPolicyConfiguration : IEntityTypeConfiguration<InventoryPolicy>
    {
        public void Configure(EntityTypeBuilder<InventoryPolicy> builder)
        {
            builder.Property(i => i.Id)
                    .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(i => i.CreatedAt)
                .HasDefaultValueSql("now()");
            // 配置索引
            builder.HasIndex(i => new { i.ProductId, i.WarehouseId }).IsUnique();
        }
    }
}
