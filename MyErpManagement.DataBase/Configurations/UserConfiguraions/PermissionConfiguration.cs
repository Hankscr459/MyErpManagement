using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.UsersModule.Entities;

namespace MyErpManagement.DataBase.Configurations.UserConfiguraions
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.Property(p => p.Id)
                    .HasDefaultValueSql("gen_random_uuid()");

            // 設定 IsActive 預設為 true (1)
            builder.Property(p => p.IsActive)
                    .HasDefaultValue(true);

            // 設定 LastSeenAt 預設為 SQL Server 的 UTC 時間
            builder.Property(p => p.LastSeenAt)
                    .HasDefaultValueSql("now()");
        }
    }
}
