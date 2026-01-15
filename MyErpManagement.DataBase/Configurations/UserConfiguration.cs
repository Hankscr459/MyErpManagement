using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using MyErpManagement.Core.Modules.UsersModule.Entities;
using MyErpManagement.DataBase.SeedData;

namespace MyErpManagement.DataBase.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        private readonly UserSeedData _userSeedData;

        public UserConfiguration(IConfiguration config)
        {
            // 通過 PasswordHasher 初始化 UserSeedData provider
            _userSeedData = new UserSeedData(config);
        }

        public void Configure(EntityTypeBuilder<User> builder)
        {
            // 讓 SQL Server 產生有順序的 Guid，提升索引效能
            // 關於 HasDefaultValueSql("NEWSEQUENTIALID()")
            // 它是給「未來」用的： 這行程式碼是在告訴 SQL Server：「以後如果有新的資料存進來，但程式碼沒給 Id 時，請你自動幫我產生一個循序的 Guid」。
            // 它是資料庫層級的規則： 只有當資料真正進到 SQL Server 儲存的那一刻，這個功能才會啟動。
            builder.Property(x => x.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()");
            // 配置索引
            builder.HasIndex(x => x.Account).IsUnique();

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // provider 提供的 Users 列表
            builder.HasData(_userSeedData.Users);
        }
    }
}