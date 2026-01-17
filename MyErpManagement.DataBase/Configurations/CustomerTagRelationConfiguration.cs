using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.CustomerModule.Entities;

namespace MyErpManagement.DataBase.Configurations
{
    internal class CustomerTagRelationConfiguration : IEntityTypeConfiguration<CustomerTagRelation>
    {
        public void Configure(EntityTypeBuilder<CustomerTagRelation> builder)
        {
            // 1. 設定複合主鍵
            builder.HasKey(ctr => new { ctr.CustomerId, ctr.CusomterTagId });

            // 2. 設定關係（通常在實體端設定過，這裡可重複確認或設定額外欄位）
            builder.Property(ctr => ctr.AssignedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
