using MyErpManagement.Core.Modules.InventoryModule.Entities;
using MyErpManagement.Core.Modules.TransferOrderModule.Enums;

namespace MyErpManagement.Core.Modules.TransferOrderModule.Entities
{
    public class TransferOrder
    {
        public Guid Id { get; set; }

        public string OrderNo { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public Guid FromWareHouseId { get; set; }
        public Guid ToWareHouseId { get; set; }

        public Guid CreatedBy { get; set; }
        public Guid? ApprovedBy { get; set; }
        public Guid? CancelBy { get; set; }
        public Guid? CompleteBy { get; set; }
        public WareHouse FromWareHouse { get; set; } = null!;
        public WareHouse ToWareHouse { get; set; } = null!;

        public TransferOrderStatusEnum Status { get; set; }

        public ICollection<TransferOrderLine> Lines { get; set; } = new List<TransferOrderLine>();
    }
}
