namespace MyErpManagement.Core.Dtos.Products.Request
{
    public class ProductListQueryRequestDto
    {
        public string? CategroyId { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }
}
