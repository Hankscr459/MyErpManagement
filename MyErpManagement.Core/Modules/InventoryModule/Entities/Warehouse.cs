namespace MyErpManagement.Core.Modules.InventoryModule.Entities
{
    public class WareHouse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
    }
}