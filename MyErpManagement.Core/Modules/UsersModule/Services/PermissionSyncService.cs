using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.UsersModule.Constants;
using MyErpManagement.Core.Modules.UsersModule.Entities;
using MyErpManagement.Core.Modules.UsersModule.IServices;
using System.Reflection;

namespace MyErpManagement.Core.Modules.UsersModule.Services
{
    public class PermissionSyncService(IUnitOfWork unitOfWork) : IPermissionSyncService
    {
        public async Task SyncAsync()
        {
            var detected = new List<(string Key, string Name)>();
            ExtractRecursive(typeof(PermissionKeysConstant), detected);

            var dbPermissions = await unitOfWork.PermissionRepository.GetAllAsync();
            var detectedKeys = detected.Select(x => x.Key).ToList();

            // 1. 處理刪除 (改為標記為停用，而非直接刪除)
            var toRemove = dbPermissions
                .Where(p => !detectedKeys.Contains(p.PermissionKey))
                .ToList();
            foreach (var p in toRemove)
            {
                p.IsActive = false;
                p.LastSeenAt = DateTime.UtcNow;
            }
            // if (toRemove.Any()) permissionRepository.RemoveRange(toRemove);

            // 2. 處理新增與更新
            foreach (var (key, name) in detected)
            {
                var existing = dbPermissions.FirstOrDefault(p => p.PermissionKey == key);
                if (existing == null)
                {
                    await unitOfWork.PermissionRepository.AddRangeAsync(new List<Permission> {
                        new() { Id = Guid.NewGuid(), PermissionKey = key, Name = name, ApiPath = "System" }
                    });
                }
                else if (existing.Name != name)
                {
                    existing.Name = name; // Entity Framework 會自動追蹤狀態，只需修改屬性
                }
            }

            await unitOfWork.Complete();
        }

        private void ExtractRecursive(Type type, List<(string Key, string Name)> result)
        {
            var keyField = type.GetField("Key", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            var nameField = type.GetField("Name", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            if (keyField != null && nameField != null)
            {
                result.Add((keyField.GetRawConstantValue()?.ToString()!, nameField.GetRawConstantValue()?.ToString()!));
            }

            foreach (var nested in type.GetNestedTypes(BindingFlags.Public | BindingFlags.Static))
            {
                ExtractRecursive(nested, result);
            }
        }
    }
}
