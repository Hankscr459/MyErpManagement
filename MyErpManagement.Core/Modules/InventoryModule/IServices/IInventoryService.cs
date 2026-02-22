using MyErpManagement.Core.Modules.InventoryModule.Entities;
using MyErpManagement.Core.Modules.InventoryModule.Models;
using MyErpManagement.Core.Modules.TransferOrderModule.Entities;

namespace MyErpManagement.Core.Modules.InventoryModule.IServices
{
    public interface IInventoryService
    {
        Task AddInventoryByCreatePurchaseOrder(InventoryModel addInventoryModel);
        Task<bool> RestoreInventoryByCancelPurchaseOrder(InventoryModel restoreInventoryModel);
        Task AddInventoryByCreateTransferOrder(TransferInventoryModel transferInventoryModel, Inventory inventory);
        Task<IEnumerable<InventoryTransaction?>> RestoreInventoryByCancelTransferOrder(Guid transferOrderId, Inventory fromInventory, Inventory toInventory);
    }
}
