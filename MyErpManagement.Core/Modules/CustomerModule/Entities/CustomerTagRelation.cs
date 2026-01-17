namespace MyErpManagement.Core.Modules.CustomerModule.Entities
{
    public class CustomerTagRelation
    {
        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = default!;

        public Guid CusomterTagId { get; set; }
        public virtual CustomerTag CustomerTag { get; set; } = default!;

        // 可以在這裡增加額外資訊
        public DateTime AssignedAt { get; set; }
    }
}
