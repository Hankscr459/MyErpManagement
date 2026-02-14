using MyErpManagement.Core.Modules.InventoryModule.Entities;
using MyErpManagement.Core.Modules.InventoryModule.Enums;
using MyErpManagement.Core.Modules.InventoryModule.IRepositories;
using MyErpManagement.Core.Modules.InventoryModule.IServices;
using MyErpManagement.Core.Modules.InventoryModule.Models;
using MyErpManagement.Core.Modules.UsersModule.Entities;

namespace MyErpManagement.Core.Modules.InventoryModule.Services
{
    public class InventoryTransactionService(IInventoryTransactionRepository inventoryTransactionRePository) : IInventoryTransactionService
    {
        public async Task AddInventoryTransaction(AddInventoryTransactionModel addInventoryTransactionModel)
        {
            await inventoryTransactionRePository.AddAsync(new InventoryTransaction
            {
                ProductId = addInventoryTransactionModel.ProductId,
                WareHouseId = addInventoryTransactionModel.WareHouseId,
                QuantityChange = addInventoryTransactionModel.QuantityChange,
                UnitCost = addInventoryTransactionModel.UnitCost,
                SourceType = addInventoryTransactionModel.SourceType,
                SourceId = addInventoryTransactionModel.SourceId,
                CreatedBy = addInventoryTransactionModel.CreatedBy,
            });
        }
    }
}
