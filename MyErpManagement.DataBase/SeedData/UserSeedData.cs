using Microsoft.Extensions.Configuration;
using MyErpManagement.Core.Modules.UsersModule.Entities;

namespace MyErpManagement.DataBase.SeedData
{
    public class UserSeedData
    {
        // 儲存 Seed Data 列表
        public IReadOnlyList<User> Users { get; }

        public UserSeedData(IConfiguration config)
        {
            // .net9之後無法直接使用以下程式碼產生 Salt 與 Hash，需改用外部工具產生後直接寫死在此處
            //var adminSalt = _passwordHasher.GenerateSalt();
            //var testSalt = _passwordHasher.GenerateSalt();

            //var adminHash = _passwordHasher.HashPassword("123456", adminSalt);
            //var testHash = _passwordHasher.HashPassword("test123", testSalt);

            Users = new List<User>
            {
                new User
                {
                    Id = Guid.Parse(config["Seed_User_All_Permission_Id"] ?? throw new Exception("Seed_User_All_Permission_Id 環境變數是空值")),
                    Account = config["Seed_User_All_Permission_Account"] ?? throw new Exception("Seed_User_All_Account 環境變數是空值"),
                    PasswordSalt = config["Seed_User_All_Permission_Salt"] ?? throw new Exception("Seed_User_All_Permission_Salt 環境變數是空值"),
                    PasswordHash = config["Seed_User_All_Permission_Hash"] ?? throw new Exception("Seed_User_All_Permission_Hash 環境變數是空值"),
                    IsSuperAdmin = true,
                },
                new User
                {
                    Id = Guid.Parse(config["Seed_User_Some_Permission_Id"] ?? throw new Exception("Seed_User_Some_Permission_Id 環境變數是空值")),
                    Account = config["Seed_User_Some_Permission_Account"] ?? throw new Exception("Seed_User_Some_Permission_Account 環境變數是空值"),
                    PasswordSalt = config["Seed_User_Some_Permission_Salt"] ?? throw new Exception("Seed_User_Some_Permission_Salt 環境變數是空值"),
                    PasswordHash = config["Seed_User_Some_Permission_Hash"] ?? throw new Exception("Seed_User_Some_Permission_Hash 環境變數是空值"),
                    IsSuperAdmin = false,
                }
            }.AsReadOnly();
        }
    }
}
