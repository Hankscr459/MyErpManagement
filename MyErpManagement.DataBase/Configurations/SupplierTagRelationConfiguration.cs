using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.SupplierModule.Entities;

namespace MyErpManagement.DataBase.Configurations
{
    public class SupplierTagRelationConfiguration : IEntityTypeConfiguration<SupplierTagRelation>
    {
        public void Configure(EntityTypeBuilder<SupplierTagRelation> builder)
        {
            // 1. 設定複合主鍵
            builder.HasKey(ctr => new { ctr.SupplierId, ctr.SupplierTagId });

            // 2. 設定關係（通常在實體端設定過，這裡可重複確認或設定額外欄位）
            builder.Property(ctr => ctr.AssignedAt)
                   .HasDefaultValueSql("now()");
        }
    }
}
