using MyErpManagement.Core.Modules.InventoryModule.Models;

namespace MyErpManagement.Core.Modules.InventoryModule.IServices
{
    public interface IInventoryService
    {
        Task AddInventoryByPurchaseOrder(AddInventoryModel addInventoryModel);
    }
}
