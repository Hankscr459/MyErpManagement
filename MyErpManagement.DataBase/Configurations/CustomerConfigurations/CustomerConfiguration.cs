using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.CustomerModule.Entities;

namespace MyErpManagement.DataBase.Configurations.CustomerConfigurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Id)
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Code)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("now()");

            builder.Property(p => p.UpdatedAt)
                   .HasDefaultValueSql("now()");

            builder.Property(p => p.Balance)
                   .HasPrecision(18, 2); // 建議設定金額精度

            // 設定與中間表的關係 (一對多，組成多對多的一環)
            builder.HasMany(c => c.CustomerTagRelations)
                   .WithOne(ctr => ctr.Customer)
                   .HasForeignKey(ctr => ctr.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 配置索引
            builder.HasIndex(p => p.Code).IsUnique();
        }
    }
}
