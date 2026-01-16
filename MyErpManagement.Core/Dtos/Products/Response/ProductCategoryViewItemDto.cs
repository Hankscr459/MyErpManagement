namespace MyErpManagement.Core.Dtos.Products.Response
{
    public class ProductCategoryViewItemDto
    {
        /// <summary>
        /// 分類Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 分類名稱
        /// </summary>
        public string Name { get; set; } = default!;
    }
}
