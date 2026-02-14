using MyErpManagement.Core.Modules.InventoryModule.Entities;
using MyErpManagement.Core.Modules.TransferOrderModule.Enums;

namespace MyErpManagement.Core.Modules.TransferOrderModule.Entities
{
    public class TransferOrder
    {
        public Guid Id { get; set; }

        public string TransferNo { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public Guid FromWareHouseId { get; set; }
        public Guid ToWareHouseId { get; set; }

        public WareHouse FromWareHouse { get; set; } = null!;
        public WareHouse ToWareHouse { get; set; } = null!;

        public TransferOrderStatusEnum Status { get; set; }

        public ICollection<TransferOrderLine> Lines { get; set; } = new List<TransferOrderLine>();
    }
}
