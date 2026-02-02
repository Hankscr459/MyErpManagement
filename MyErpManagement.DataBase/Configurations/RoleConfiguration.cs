using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyErpManagement.DataBase.SeedData;
using MyErpManagement.Core.Modules.UsersModule.Entities;

namespace MyErpManagement.DataBase.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        private readonly RoleSeedData _roleSeedData;
        public RoleConfiguration()
        {
            _roleSeedData = new RoleSeedData();
        }

        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // 設定 Role 與 Permission 的預設值或索引(可視需求移至獨立的 Configuration 類別)
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

            
            builder.HasData(_roleSeedData.Role);
        }
    }
}
