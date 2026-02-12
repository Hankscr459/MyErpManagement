using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.InventoryModule.Entities;

namespace MyErpManagement.DataBase.Configurations
{
    public class WareHouseConfiguration : IEntityTypeConfiguration<WareHouse>
    {
        public void Configure(EntityTypeBuilder<WareHouse> builder)
        {
            builder.Property(wh => wh.Id)
                    .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(wh => wh.CreatedAt)
                .HasDefaultValueSql("now()");
            // 配置索引
            builder.HasIndex(wh => wh.Code).IsUnique();
        }
    }
}
