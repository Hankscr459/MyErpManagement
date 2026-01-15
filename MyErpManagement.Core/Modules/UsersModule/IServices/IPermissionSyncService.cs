namespace MyErpManagement.Core.Modules.UsersModule.IServices
{
    public interface IPermissionSyncService
    {
        /// <summary>
        /// 同步程式碼中定義的權限清單至資料庫
        /// </summary>
        Task SyncAsync();
    }
}
