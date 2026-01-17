namespace MyErpManagement.Core.Modules.CustomerModule.Entities
{
    
    public class CustomerTag
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public Guid CreatedBy { get; set; }

        // 關聯回中間表
        public virtual ICollection<CustomerTagRelation> CustomerTagRelations { get; set; } = new List<CustomerTagRelation>();
    }
}
