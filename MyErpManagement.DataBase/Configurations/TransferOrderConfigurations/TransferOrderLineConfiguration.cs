using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.TransferOrderModule.Entities;

namespace MyErpManagement.DataBase.Configurations.TransferOrderConfigurations
{
    public class TransferOrderLineConfiguration : IEntityTypeConfiguration<TransferOrderLine>
    {
        public void Configure(EntityTypeBuilder<TransferOrderLine> builder)
        {
            builder.Property(pol => pol.Id)
                    .HasDefaultValueSql("gen_random_uuid()");
        }
    }
}
