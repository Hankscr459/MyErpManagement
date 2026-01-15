namespace MyErpManagement.Core.Modules.UsersModule.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
