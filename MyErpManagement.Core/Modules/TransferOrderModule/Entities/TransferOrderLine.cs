using MyErpManagement.Core.Modules.ProductsModule.Entities;

namespace MyErpManagement.Core.Modules.TransferOrderModule.Entities
{
    public class TransferOrderLine
    {
        public Guid Id { get; set; }

        public Guid TransferOrderId { get; set; }
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public TransferOrder TransferOrder { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
