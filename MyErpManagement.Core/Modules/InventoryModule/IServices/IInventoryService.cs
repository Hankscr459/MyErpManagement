using MyErpManagement.Core.Modules.InventoryModule.Entities;
using MyErpManagement.Core.Modules.InventoryModule.Models;

namespace MyErpManagement.Core.Modules.InventoryModule.IServices
{
    public interface IInventoryService
    {
        /// <summary>
        /// 採購單新增庫存
        /// </summary>
        /// <param name="addInventoryModel"></param>
        /// <param name="inventory">此倉庫的商品庫存</param>
        /// <returns></returns>
        Task AddInventoryByPurchaseOrder(InventoryModel addInventoryModel, Inventory? inventory);
        /// <summary>
        /// 還原核准後的採購單
        /// </summary>
        /// <param name="restoreInventoryModel"></param>
        /// <param name="inventory">此倉庫的商品庫存</param>
        /// <returns></returns>
        Task RestoreInventoryByPurchaseOrder(InventoryModel restoreInventoryModel, Inventory inventory);
        /// <summary>
        /// 新增庫存
        /// </summary>
        /// <param name="transferInventoryModel"></param>
        /// <param name="fromInventory">出庫的庫存</param>
        /// <param name="toInventory">入庫的庫存</param>
        /// <returns></returns>
        Task AddInventoryByCreateTransferOrder(TransferInventoryModel transferInventoryModel, Inventory fromInventory, Inventory? toInventory);
        /// <summary>
        /// 還原核准後的調貨單,
        /// 還原的庫存金額會有意點誤差,因為採用平均成本法,所以會以目前庫存的平均成本為基礎去還原金額,不過數量是正確的
        /// </summary>
        /// <param name="transferOrderId">調貨單Id</param>
        /// <param name="fromInventory">核准後出庫的庫存</param>
        /// <param name="toInventory">核准後入庫的庫存</param>
        /// <returns></returns>
        Task<IEnumerable<InventoryTransaction?>> RestoreInventoryByCancelTransferOrder(Guid transferOrderId, Inventory fromInventory, Inventory toInventory);
    }
}
