namespace MyErpManagement.Core.Dtos.PurchaseOrder.Request
{
    public class CreatePurchaseOrderLineRequestDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Decimal Price { get; set; }
    }
}
