namespace MyErpManagement.Core.Dtos.TransferOrder.Request
{
    public class CreateTransferOrderLineRequestDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
