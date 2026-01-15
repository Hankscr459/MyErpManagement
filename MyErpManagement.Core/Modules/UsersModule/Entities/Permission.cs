namespace MyErpManagement.Core.Modules.UsersModule.Entities
{
    public class Permission
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!; // 例如 "GetUsers", "DeleteUser"

        // 這是 Attribute 會比對的關鍵字，如 "User_Read"
        public string PermissionKey { get; set; } = default!;

        public string? ApiPath { get; set; } // 對應的 API 路徑或標記

        public bool IsActive { get; set; } = true;
        public DateTime LastSeenAt { get; set; }

        // 必須加入此屬性，才能與中間表關聯
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
