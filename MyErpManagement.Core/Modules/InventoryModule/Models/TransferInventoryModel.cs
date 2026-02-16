namespace MyErpManagement.Core.Modules.InventoryModule.Models
{
    public class TransferInventoryModel
    {
        public Guid ProductId { get; set; }
        public Guid FromWareHouseId { get; set; }
        public Guid ToWareHouseId { get; set; }
        public int Quantity { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
