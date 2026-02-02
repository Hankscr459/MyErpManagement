using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.CustomerModule.Entities;

namespace MyErpManagement.DataBase.Configurations
{
    public class CustomerTagConfiguration : IEntityTypeConfiguration<CustomerTag>
    {
        public void Configure(EntityTypeBuilder<CustomerTag> builder)
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
            builder.HasMany(t => t.CustomerTagRelations)
                   .WithOne(ctr => ctr.CustomerTag)
                   .HasForeignKey(ctr => ctr.CusomterTagId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
