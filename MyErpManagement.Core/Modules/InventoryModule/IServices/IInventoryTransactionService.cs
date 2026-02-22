using MyErpManagement.Core.Modules.InventoryModule.Entities;
using MyErpManagement.Core.Modules.InventoryModule.Models;

namespace MyErpManagement.Core.Modules.InventoryModule.IServices
{
    public interface IInventoryTransactionService
    {
        Task AddInventoryTransaction(AddInventoryTransactionModel addInventoryTransactionModel);
        Task AddInventoryTransactionByTransferOrder(TransferInventoryTransactionModel addInventoryTransactionModel, Inventory fromInventory);
    }
}
