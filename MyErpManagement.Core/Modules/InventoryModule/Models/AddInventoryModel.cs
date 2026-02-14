namespace MyErpManagement.Core.Modules.InventoryModule.Models
{
    public class AddInventoryModel
    {
        public Guid ProductId { get; set; }
        public Guid WareHouseId { get; set; }
        public int Quantity { get; set; }
        public Decimal Price { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
