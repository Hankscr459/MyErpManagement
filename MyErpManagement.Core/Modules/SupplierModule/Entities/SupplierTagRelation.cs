namespace MyErpManagement.Core.Modules.SupplierModule.Entities
{
    public class SupplierTagRelation
    {
        public Guid SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; } = default!;

        public Guid SupplierTagId { get; set; }
        public virtual SupplierTag SupplierTag { get; set; } = default!;

        // 可以在這裡增加額外資訊
        public DateTimeOffset AssignedAt { get; set; }
    }
}
