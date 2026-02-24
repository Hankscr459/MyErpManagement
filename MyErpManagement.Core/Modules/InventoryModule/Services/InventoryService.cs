using MyErpManagement.Core.Modules.InventoryModule.Entities;
using MyErpManagement.Core.Modules.InventoryModule.IRepositories;
using MyErpManagement.Core.Modules.InventoryModule.IServices;
using MyErpManagement.Core.Modules.InventoryModule.Models;

namespace MyErpManagement.Core.Modules.InventoryModule.Services
{
    public class InventoryService(IInventoryRepository inventoryRepository, IInventoryTransactionRepository inventoryTransactionRepository) : IInventoryService
    {
        public async Task AddInventoryByPurchaseOrder(InventoryModel addInventoryModel, Inventory? inventory)
        {
            if (inventory is not null)
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

        public async Task RestoreInventoryByPurchaseOrder(InventoryModel restoreInventoryModel, Inventory inventory)
        {
            var quantity = inventory.Quantity - restoreInventoryModel?.Quantity ?? 0;
            inventory.Quantity = quantity;
            if (inventory.Quantity is 0)
            {
                inventory.AverageCost = 0;
            }
            else
            {
                inventory.AverageCost = (inventory.AverageCost * inventory.Quantity - restoreInventoryModel?.Price ?? 0 * restoreInventoryModel.Quantity) / quantity;
            }

            inventoryRepository.Update(inventory);
        }

        public async Task AddInventoryByTransferOrder(TransferInventoryModel transferInventoryModel, Inventory fromInventory, Inventory? toInventory)
        {
            fromInventory.Quantity = fromInventory.Quantity - transferInventoryModel?.Quantity ?? 0;
            inventoryRepository.Update(fromInventory);
            if (toInventory is null)
            {
                await inventoryRepository.AddAsync(new Inventory
                {
                    ProductId = transferInventoryModel!.ProductId,
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

        public async Task<IEnumerable<InventoryTransaction?>> RestoreInventoryByTransferOrder(Guid transferOrderId, Inventory fromInventory, Inventory toInventory)
        {
            /***
             * 還原出庫數量，
             * 還原AverageCost的值是採用平均值的方式計算，
             * 因為在轉移單的入庫時，已經將成本計算好並寫入InventoryTransaction了，
             * 所以在還原的時候，可以直接從InventoryTransaction中拿到成本金額，然後用平均值的方式計算出還原後的AverageCost。
             * ***/
            // 此調貨單出庫紀錄
            var inventoryOut = await inventoryTransactionRepository.GetFirstOrDefaultAsync(it => it.SourceId == transferOrderId && it.SourceType == Enums.InventorySourceTypeEnum.TransferOrderOut);
            /*** 核准前出庫的倉庫商品數量 = 核准後出庫的倉庫商品數量 + 此調貨單出庫紀錄的數量 * -1(乘-1是因為出庫紀錄的數量本身是負值) ***/
            // 核准前出庫的倉庫商品數量
            var fromInventoryQuantity = fromInventory.Quantity - (inventoryOut?.QuantityChange ?? 0) * -1;
            // 此調貨單出庫紀錄的金額 = 此調貨單出庫紀錄的數量 * 此調貨單出庫紀錄的平均單價
            var inventoryOutCountPrice = inventoryOut?.QuantityChange * inventoryOut?.UnitCost ?? 0;

            /***
             * inventoryOutCountPrice * -1(乘-1是因為出庫紀錄的數量本身是負值)
             * ***/
            // 核准前出庫的倉庫商品平均單價 = (核准後出庫的倉庫商品平均單價 * 核准後出庫的倉庫商品數量 - 此調貨單出庫紀錄的金額 * -1) / 核准前出庫的倉庫商品數量
            var fromInventoryAverageCost = (fromInventory.AverageCost * fromInventory.Quantity + inventoryOutCountPrice * -1) / fromInventoryQuantity;
            
            fromInventory.Quantity = fromInventoryQuantity;
            fromInventory.AverageCost = fromInventoryAverageCost;
            inventoryRepository.Update(fromInventory);

            /***
             * 還原入庫數量，
             * 還原AverageCost的值是採用平均值的方式計算
             * ***/
            // 此調貨單入庫紀錄
            var inventoryIn = await inventoryTransactionRepository.GetFirstOrDefaultAsync(it => it.SourceId == transferOrderId && it.SourceType == Enums.InventorySourceTypeEnum.TransferOrderIn);
            
            /*** 核准前入庫的倉庫商品數量 = 核准後入庫的倉庫商品數量 - 此調貨單入庫紀錄的數量 ***/
            // 核准前入庫的倉庫商品數量
            var toInventoryQuantity = toInventory.Quantity - inventoryIn?.QuantityChange ?? 0;
            decimal toInventoryAverageCost = 0;
            if (toInventoryQuantity is not 0)
            {
                /***
                 * 計算核准前入庫的倉庫商品平均單價
                 * (toInventory.AverageCost * toInventory.Quantity - (inventoryIn?.QuantityChange ?? 0 * inventoryIn?.UnitCost ?? 0) / toInventoryQuantity);
                 * 核准後入庫的倉庫商品平均單價(Inventory.AverageCost) * 核准後入庫的倉庫商品數量 - 此調貨單入庫紀錄的數量(InventoryTransaction.QuantityChange) * 此調貨單入庫紀錄的平均單價 / 還原後的數量 = 還原後的平均單價
                 * 這裡是因為在調貨單入庫的時候，已經將成本計算好並寫入InventoryTransaction了，所以在還原的時候，可以直接從InventoryTransaction中拿到成本金額，然後用平均值的方式計算出還原後的AverageCost。
                 * **/
                toInventoryAverageCost = (toInventory.AverageCost * toInventory.Quantity - (inventoryIn?.QuantityChange ?? 0 * inventoryIn?.UnitCost ?? 0) / toInventoryQuantity);
            }
            toInventory.Quantity = toInventoryQuantity;
            toInventory.AverageCost = toInventoryAverageCost;
            inventoryRepository.Update(toInventory);

            return new List<InventoryTransaction?> { inventoryOut, inventoryIn };
        }
    }
}
