using MyErpManagement.Core.Modules.InventoryModule.Entities;
using MyErpManagement.Core.Modules.InventoryModule.IRepositories;
using MyErpManagement.Core.Modules.InventoryModule.IServices;
using MyErpManagement.Core.Modules.InventoryModule.Models;

namespace MyErpManagement.Core.Modules.InventoryModule.Services
{
    public class InventoryService(IInventoryRepository inventoryRepository) : IInventoryService
    {
        public async Task AddInventoryByCreatePurchaseOrder(InventoryModel addInventoryModel)
        {
            var inventory = await inventoryRepository.GetFirstOrDefaultAsync(
                i => i.WarehouseId == addInventoryModel.WareHouseId && i.ProductId == addInventoryModel.ProductId
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
                    WarehouseId = addInventoryModel.WareHouseId,
                    Quantity = addInventoryModel.Quantity,
                    AverageCost = addInventoryModel.Price,
                    CreatedBy = addInventoryModel.CreatedBy,
                });
            }
        }

        public async Task<bool> RestoreInventoryByCancelPurchaseOrder(InventoryModel restoreInventoryModel)
        {
            var inventory = await inventoryRepository.GetFirstOrDefaultAsync(
                i => i.WarehouseId == restoreInventoryModel.WareHouseId && i.ProductId == restoreInventoryModel.ProductId
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

        public async Task<bool> AddInventoryByCreateTransferOrder(TransferInventoryModel transferInventoryModel)
        {
            var fromInventory = await inventoryRepository.GetFirstOrDefaultAsync(
                i => i.WarehouseId == transferInventoryModel.FromWareHouseId && i.ProductId == transferInventoryModel.ProductId
            );
            if (fromInventory is null)
            {
                return false;
            }
            fromInventory.Quantity = fromInventory.Quantity - transferInventoryModel?.Quantity ?? 0;
            inventoryRepository.Update(fromInventory);
            var toInventory = await inventoryRepository.GetFirstOrDefaultAsync(
                i => i.WarehouseId == transferInventoryModel.ToWareHouseId && i.ProductId == transferInventoryModel.ProductId
            );
            if (toInventory is null)
            {
                await inventoryRepository.AddAsync(new Inventory
                {
                    ProductId = transferInventoryModel.ProductId,
                    WarehouseId = transferInventoryModel.ToWareHouseId,
                    Quantity = transferInventoryModel.Quantity,
                    AverageCost = fromInventory.AverageCost,
                    CreatedBy = transferInventoryModel.CreatedBy,
                });
            }
            else
            {
                var toQuantity = toInventory.Quantity + transferInventoryModel?.Quantity ?? 0;
                toInventory.AverageCost = (toInventory.AverageCost * toInventory.Quantity + fromInventory.AverageCost * transferInventoryModel.Quantity) / toQuantity;
                inventoryRepository.Update(toInventory);
            }
                return true;
        }
    }
}
