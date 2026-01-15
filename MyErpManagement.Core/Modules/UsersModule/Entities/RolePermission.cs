namespace MyErpManagement.Core.Modules.UsersModule.Entities
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }

        public Guid PermissionId { get; set; }
        public virtual Role Role { get; set; } = default!;

        public virtual Permission Permission { get; set; } = default!;
    }
}
