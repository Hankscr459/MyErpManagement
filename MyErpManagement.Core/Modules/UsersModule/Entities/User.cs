using System.ComponentModel.DataAnnotations;

namespace MyErpManagement.Core.Modules.UsersModule.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Account { get; set; } = default!;
        [EmailAddress]
        public string? Email { get; set; }
        public string PasswordHash { get; set; } = default!;
        public string PasswordSalt { get; set; } = default!;

        public bool IsSuperAdmin { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        // 多對多：一個使用者可以擁有多個角色關聯
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
