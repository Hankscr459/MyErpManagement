using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.PurchaseOrderModule.Entities;

namespace MyErpManagement.DataBase.Configurations
{
    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder.Property(po => po.Id)
                    .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(po => po.CreatedAt)
                .HasDefaultValueSql("now()");
            builder.Property(po => po.UpdatedAt)
                .HasDefaultValueSql("now()");
            builder.HasIndex(x => x.OrderNo).IsUnique();
            builder.HasMany(x => x.Lines)
             .WithOne(x => x.PurchaseOrder)
             .HasForeignKey(x => x.PurchaseOrderId);
        }
    }
}
