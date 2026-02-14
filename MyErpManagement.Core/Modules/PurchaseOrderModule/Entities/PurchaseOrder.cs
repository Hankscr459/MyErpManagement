using MyErpManagement.Core.Modules.InventoryModule.Entities;
using MyErpManagement.Core.Modules.PurchaseOrderModule.Enums;
using MyErpManagement.Core.Modules.SupplierModule.Entities;

namespace MyErpManagement.Core.Modules.PurchaseOrderModule.Entities
{
    public class PurchaseOrder
    {
        public Guid Id { get; set; }
        public string OrderNo { get; set; } = null!;
        public Guid WareHouseId { get; set; }
        public Guid SupplierId { get; set; }
        public Decimal TotalPrice { get; set; }
        public PurchaseOrderStatusEnum Status { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? ApprovedBy { get; set; }
        public Guid? CancelBy { get; set; }
        public Guid? CompleteBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public WareHouse WareHouse { get; set; } = null!;
        public Supplier Supplier { get; set; } = null!;

        public ICollection<PurchaseOrderLine> Lines { get; set; } = new List<PurchaseOrderLine>();
    }
}
