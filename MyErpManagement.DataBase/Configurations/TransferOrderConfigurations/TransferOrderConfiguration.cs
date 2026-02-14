using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.TransferOrderModule.Entities;

namespace MyErpManagement.DataBase.Configurations.TransferOrderConfigurations
{
    public class TransferOrderConfiguration : IEntityTypeConfiguration<TransferOrder>
    {
        public void Configure(EntityTypeBuilder<TransferOrder> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(t => t.Id)
                   .HasDefaultValueSql("gen_random_uuid()");
            builder.HasIndex(t => t.TransferNo).IsUnique();

            builder.HasMany(t => t.Lines)
             .WithOne(t => t.TransferOrder)
             .HasForeignKey(t => t.TransferOrderId);

            builder.HasOne(t => t.FromWareHouse)
             .WithMany()
             .HasForeignKey(t => t.FromWareHouseId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.ToWareHouse)
             .WithMany()
             .HasForeignKey(t => t.ToWareHouseId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
