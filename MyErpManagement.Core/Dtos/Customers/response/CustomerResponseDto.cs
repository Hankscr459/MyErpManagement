namespace MyErpManagement.Core.Dtos.Customers.response
{
    public class CustomerResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Notes { get; set; }

        /// <summary>
        /// 多個標籤物件
        /// </summary>
        public List<CustomerTagResponseDto> CustomerTags { get; set; } = new();
    }
}
