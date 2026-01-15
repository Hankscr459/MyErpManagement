namespace MyErpManagement.Core.Dtos.Products.Request
{
    public class CreateProductCategoryRequestDto
    {
        public string Name { get; set; } = default!;
        public Guid ParentId { get; set; }
    }
}
