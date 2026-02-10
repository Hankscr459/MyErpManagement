using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.OrderSequenceModule.Entities;

namespace MyErpManagement.DataBase.Configurations
{
    public class OrderSequenceConfiguration : IEntityTypeConfiguration<OrderSequence>
    {
        public void Configure(EntityTypeBuilder<OrderSequence> builder)
        {
            builder.HasKey(e => new { e.OrderType, e.Period });

            builder.Property(e => e.OrderType)
                  .HasMaxLength(10)
                  .IsRequired();

            builder.Property(e => e.Period)
                  .IsFixedLength()
                  .HasMaxLength(6)
                  .IsRequired();

            builder.Property(e => e.CurrentNo);
        }
    }
}
