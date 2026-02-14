using MyErpManagement.Core.Modules.InventoryModule.Entities;
using MyErpManagement.Core.Modules.InventoryModule.IRepositories;
using MyErpManagement.Core.Modules.InventoryModule.IServices;
using MyErpManagement.Core.Modules.InventoryModule.Models;
using MyErpManagement.Core.Modules.OrderNoModule.IRepositories;

namespace MyErpManagement.Core.Modules.InventoryModule.Services
{
    public class InventoryService(IInventoryRepository inventoryRepository) : IInventoryService
    {
        public async Task AddInventoryByPurchaseOrder(AddInventoryModel addInventoryModel)
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
    }
}
