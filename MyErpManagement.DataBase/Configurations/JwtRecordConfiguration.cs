using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.JwtModule.Entities;

namespace MyErpManagement.DataBase.Configurations
{
    public class JwtRecordConfiguration : IEntityTypeConfiguration<JwtRecord>
    {
        public void Configure(EntityTypeBuilder<JwtRecord> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                    .HasDefaultValueSql("gen_random_uuid()");
            // 設定與 User 的關係 (非強關聯亦可，視需求而定)
            builder.HasIndex(x => x.UserId);
            builder.Property(x => x.TokenValue).IsRequired();
        }
    }
}
