namespace MyErpManagement.Core.Dtos.Products.Request
{
    public class CreateProductRequestDto
    {
        /// <summary>
        /// 商品分類
        /// </summary>
        public Guid ProductCategoryId { get; set; }
        /// <summary>
        /// 編碼
        /// </summary>
        public string Code { get; set; } = default!;

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; } = default!;
        /// <summary>
        /// 商品規格
        /// </summary>
        public string? Specification { get; set; }

        /// <summary>
        /// 進貨單價
        /// </summary>
        public double PurchasePrice { get; set; } = default!;

        /// <summary>
        /// 銷貨單價
        /// </summary>
        public double SalesPrice { get; set; } = default!;
    }
}
