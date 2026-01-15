using MyErpManagement.Core.Modules.UsersModule.Entities;

namespace MyErpManagement.DataBase.SeedData
{
    public class RoleSeedData
    {
        public Role Role { get; set; }
        public RoleSeedData()
        {
            // 插入預設角色，例如 Admin
            var adminId = Guid.Parse("D2753566-52A4-1512-56D1-A14132173B47");
            Role = new Role { Id = adminId, Name = "Admin" };
        }
    }
}
