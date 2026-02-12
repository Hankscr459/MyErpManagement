using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.PurchaseOrderModule.Entities;

namespace MyErpManagement.DataBase.Configurations
{
    public class PurchaseOrderLineConfiguration : IEntityTypeConfiguration<PurchaseOrderLine>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderLine> builder)
        {
            builder.Property(pol => pol.Id)
                    .HasDefaultValueSql("gen_random_uuid()");
        }
    }
}
