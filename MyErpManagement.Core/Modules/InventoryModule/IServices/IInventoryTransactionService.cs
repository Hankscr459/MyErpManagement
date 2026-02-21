using MyErpManagement.Core.Modules.InventoryModule.Models;

namespace MyErpManagement.Core.Modules.InventoryModule.IServices
{
    public interface IInventoryTransactionService
    {
        Task AddInventoryTransaction(AddInventoryTransactionModel addInventoryTransactionModel);
        Task<bool> AddInventoryTransactionByTransferOrder(TransferInventoryTransactionModel addInventoryTransactionModel);
    }
}
