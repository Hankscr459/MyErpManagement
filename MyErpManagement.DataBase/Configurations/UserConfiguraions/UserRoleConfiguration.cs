using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyErpManagement.Core.Modules.UsersModule.Entities;

namespace MyErpManagement.DataBase.Configurations.UserConfiguraions
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        // 設定 UserRole 中間表與複合主鍵
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });

            // 設定一對多關係組成多對多
            builder.HasOne(ur => ur.User)
                   .WithMany(u => u.UserRoles)
                   .HasForeignKey(ur => ur.UserId);

            builder.HasOne(ur => ur.Role)
                   .WithMany(r => r.UserRoles)
                   .HasForeignKey(ur => ur.RoleId);
        }
    }
}
