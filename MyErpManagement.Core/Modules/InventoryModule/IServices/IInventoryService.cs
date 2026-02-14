using MyErpManagement.Core.Modules.InventoryModule.Models;

namespace MyErpManagement.Core.Modules.InventoryModule.IServices
{
    public interface IInventoryService
    {
        Task AddInventoryByCreatePurchaseOrder(InventoryModel addInventoryModel);
        Task<bool> RestoreInventoryByCancelPurchaseOrder(InventoryModel restoreInventoryModel);
    }
}
