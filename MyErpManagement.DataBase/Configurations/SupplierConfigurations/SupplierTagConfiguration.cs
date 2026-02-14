using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.SupplierModule.Entities;

namespace MyErpManagement.DataBase.Configurations.SupplierConfigurations
{
    public class SupplierTagConfiguration : IEntityTypeConfiguration<SupplierTag>
    {
        public void Configure(EntityTypeBuilder<SupplierTag> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Id)
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.CreateAt)
                   .HasDefaultValueSql("now()");

            builder.Property(p => p.UpdateAt)
                   .HasDefaultValueSql("now()");

            // 設定與中間表的關係
            builder.HasMany(t => t.SupplierTagRelations)
                   .WithOne(ctr => ctr.SupplierTag)
                   .HasForeignKey(ctr => ctr.SupplierTagId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ct => ct.Name).IsUnique();
        }
    }
}
