using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.CustomerModule.Entities;

namespace MyErpManagement.DataBase.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()");
            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(p => p.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
