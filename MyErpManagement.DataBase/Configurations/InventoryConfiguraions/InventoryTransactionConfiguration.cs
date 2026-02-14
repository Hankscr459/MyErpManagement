using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.InventoryModule.Entities;

namespace MyErpManagement.DataBase.Configurations.InventoryConfiguraions
{
    public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
    {
        public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
        {
            builder.Property(i => i.Id)
                    .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(i => i.CreatedAt)
                .HasDefaultValueSql("now()");
        }
    }
}
