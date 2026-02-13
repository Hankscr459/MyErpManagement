using MyErpManagement.Core.Modules.ProductsModule.Entities;

namespace MyErpManagement.Core.Modules.PurchaseOrderModule.Entities
{
    public class PurchaseOrderLine
    {
        public Guid Id { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 0;
        public decimal Price { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
