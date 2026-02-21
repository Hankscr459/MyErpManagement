using MyErpManagement.Core.Modules.InventoryModule.Entities;
using MyErpManagement.Core.Modules.InventoryModule.Enums;
using MyErpManagement.Core.Modules.InventoryModule.IRepositories;
using MyErpManagement.Core.Modules.InventoryModule.IServices;
using MyErpManagement.Core.Modules.InventoryModule.Models;
using MyErpManagement.Core.Modules.UsersModule.Entities;

namespace MyErpManagement.Core.Modules.InventoryModule.Services
{
    public class InventoryTransactionService(IInventoryTransactionRepository inventoryTransactionRePository, IInventoryRepository inventoryRePository) : IInventoryTransactionService
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
                TotalCost = addInventoryTransactionModel.UnitCost * addInventoryTransactionModel.QuantityChange,
            });
        }

        public async Task<bool> AddInventoryTransactionByTransferOrder(TransferInventoryTransactionModel addInventoryTransactionModel)
        {
            var fromInventory = await inventoryRePository.GetFirstOrDefaultAsync(
                i => i.WareHouseId == addInventoryTransactionModel.FromWareHouseId && i.ProductId == addInventoryTransactionModel.ProductId
            );
            if (fromInventory is null)
            {
                return false; 
            }

            await inventoryTransactionRePository.AddAsync(new InventoryTransaction
            {
                ProductId = addInventoryTransactionModel.ProductId,
                WareHouseId = addInventoryTransactionModel.FromWareHouseId,
                QuantityChange = -addInventoryTransactionModel.QuantityChange,
                UnitCost = fromInventory.AverageCost,
                SourceType = InventorySourceTypeEnum.TransferOrderOut,
                SourceId = addInventoryTransactionModel.SourceId,
                CreatedBy = addInventoryTransactionModel.CreatedBy,
                TotalCost = fromInventory.AverageCost * addInventoryTransactionModel.QuantityChange,
            });
            await inventoryTransactionRePository.AddAsync(new InventoryTransaction
            {
                ProductId = addInventoryTransactionModel.ProductId,
                WareHouseId = addInventoryTransactionModel.FromWareHouseId,
                QuantityChange = addInventoryTransactionModel.QuantityChange,
                UnitCost = fromInventory.AverageCost,
                SourceType = InventorySourceTypeEnum.TransferOrderIn,
                SourceId = addInventoryTransactionModel.SourceId,
                CreatedBy = addInventoryTransactionModel.CreatedBy,
                TotalCost = fromInventory.AverageCost * addInventoryTransactionModel.QuantityChange,
            });
            return true;
        }
    }
}
