using MyErpManagement.Core.Modules.InventoryModule.Entities;
using MyErpManagement.Core.Modules.InventoryModule.IRepositories;
using MyErpManagement.Core.Modules.InventoryModule.IServices;
using MyErpManagement.Core.Modules.InventoryModule.Models;

namespace MyErpManagement.Core.Modules.InventoryModule.Services
{
    public class InventoryService(IInventoryRepository inventoryRepository, IInventoryTransactionRepository inventoryTransactionRepository) : IInventoryService
    {
        public async Task AddInventoryByCreatePurchaseOrder(InventoryModel addInventoryModel)
        {
            var inventory = await inventoryRepository.GetFirstOrDefaultAsync(
                i => i.WareHouseId == addInventoryModel.WareHouseId && i.ProductId == addInventoryModel.ProductId
            );
            var averageCost = addInventoryModel.Price;
            if (inventory != null)
            { 
                var inventoryCountPrice = inventory.AverageCost * inventory.Quantity;
                var newCountPrice = addInventoryModel?.Price * addInventoryModel?.Quantity ?? 0;
                inventory.AverageCost = (inventoryCountPrice + newCountPrice) / (inventory.Quantity + addInventoryModel?.Quantity ?? 0);
                inventory.Quantity = inventory.Quantity + addInventoryModel?.Quantity ?? 0;
                inventoryRepository.Update(inventory);
            }
            else
            {
                await inventoryRepository.AddAsync(new Inventory { 
                    ProductId = addInventoryModel.ProductId,
                    WareHouseId = addInventoryModel.WareHouseId,
                    Quantity = addInventoryModel.Quantity,
                    AverageCost = addInventoryModel.Price,
                    CreatedBy = addInventoryModel.CreatedBy,
                });
            }
        }

        public async Task<bool> RestoreInventoryByCancelPurchaseOrder(InventoryModel restoreInventoryModel)
        {
            var inventory = await inventoryRepository.GetFirstOrDefaultAsync(
                i => i.WareHouseId == restoreInventoryModel.WareHouseId && i.ProductId == restoreInventoryModel.ProductId
            );
            if (inventory == null)
            {
                return false;
            }
            var quantity = inventory.Quantity - restoreInventoryModel?.Quantity ?? 0;
            inventory.Quantity = quantity;
            if (inventory.Quantity == 0)
            {
                inventory.AverageCost = 0;
            }
            else
            {
                inventory.AverageCost = (inventory.AverageCost * inventory.Quantity - restoreInventoryModel?.Price ?? 0 * restoreInventoryModel.Quantity) / quantity;
            }

            inventoryRepository.Update(inventory);
            return true;
        }

        public async Task AddInventoryByCreateTransferOrder(TransferInventoryModel transferInventoryModel, Inventory fromInventory)
        {
            fromInventory.Quantity = fromInventory.Quantity - transferInventoryModel?.Quantity ?? 0;
            inventoryRepository.Update(fromInventory);
            var toInventory = await inventoryRepository.GetFirstOrDefaultAsync(
                i => i.WareHouseId == transferInventoryModel.ToWareHouseId && i.ProductId == transferInventoryModel.ProductId
            );
            if (toInventory is null)
            {
                await inventoryRepository.AddAsync(new Inventory
                {
                    ProductId = transferInventoryModel.ProductId,
                    WareHouseId = transferInventoryModel.ToWareHouseId,
                    Quantity = transferInventoryModel.Quantity,
                    AverageCost = fromInventory.AverageCost,
                    CreatedBy = transferInventoryModel.CreatedBy,
                });
            }
            else
            {
                var toQuantity = toInventory.Quantity + transferInventoryModel?.Quantity ?? 0;
                toInventory.AverageCost = (toInventory.AverageCost * toInventory.Quantity + fromInventory.AverageCost * transferInventoryModel.Quantity) / toQuantity;
                toInventory.Quantity = toQuantity;
                inventoryRepository.Update(toInventory);
            }
        }

        public async Task<IEnumerable<InventoryTransaction?>> RestoreInventoryByCancelTransferOrder(Guid transferOrderId, Inventory fromInventory, Inventory toInventory)
        {
            var inventoryOut = await inventoryTransactionRepository.GetFirstOrDefaultAsync(it => it.SourceId == transferOrderId && it.SourceType == Enums.InventorySourceTypeEnum.TransferOrderOut);
            var fromInventoryQuantity = fromInventory.Quantity + (inventoryOut?.QuantityChange ?? 0) * -1;
            var inventoryOutCountPrice = inventoryOut?.QuantityChange * inventoryOut?.UnitCost ?? 0;
            var fromInventoryAverageCost = (fromInventory.AverageCost * fromInventory.Quantity + inventoryOutCountPrice * -1) / fromInventoryQuantity;
            fromInventory.Quantity = fromInventoryQuantity;
            fromInventory.AverageCost = fromInventoryAverageCost;
            inventoryRepository.Update(fromInventory);

            var inventoryIn = await inventoryTransactionRepository.GetFirstOrDefaultAsync(it => it.SourceId == transferOrderId && it.SourceType == Enums.InventorySourceTypeEnum.TransferOrderIn);
            var toInventoryQuantity = toInventory.Quantity - inventoryIn?.QuantityChange ?? 0;
            decimal toInventoryAverageCost = 0;
            if (toInventoryQuantity != 0)
            {
                toInventoryAverageCost = (toInventory.AverageCost * toInventory.Quantity - (inventoryIn?.QuantityChange ?? 0 * inventoryIn?.UnitCost ?? 0) / toInventoryQuantity);
            }
            toInventory.Quantity = toInventoryQuantity;
            toInventory.AverageCost = toInventoryAverageCost;
            inventoryRepository.Update(toInventory);
            return new List<InventoryTransaction?> { inventoryOut, inventoryIn };
        }
    }
}
