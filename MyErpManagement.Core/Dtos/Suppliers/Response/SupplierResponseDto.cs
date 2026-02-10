namespace MyErpManagement.Core.Dtos.Suppliers.Response
{
    public class SupplierResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public decimal Balance { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string? Notes { get; set; }

        /// <summary>
        /// 多個標籤物件
        /// </summary>
        public List<SupplierTagResponseDto> SupplierTags { get; set; } = new();
    }
}
