namespace MyErpManagement.Core.Dtos.Products.Response
{
    public class ProductCategoryResponseDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 父類別 ID
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 排序使用
        /// </summary>
        public int SortOrder { get; set; } = 0;
        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// 建立人
        /// </summary>
        public Guid? CreatedBy { get; set; }
    }
}
