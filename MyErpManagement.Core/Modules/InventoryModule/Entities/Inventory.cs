using MyErpManagement.Core.Modules.ProductsModule.Entities;

namespace MyErpManagement.Core.Modules.InventoryModule.Entities
{
    public class Inventory
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid WarehouseId { get; set; }
        public int Quantity { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual WareHouse WareHouse { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public decimal AverageCost { get; set; }
    }
}
