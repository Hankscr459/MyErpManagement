using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyErpManagement.Core.Modules.ProductsModule.Entities
{
    public class ProductCategory
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [Required]
        [MaxLength(100)]
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

        // --- 導覽屬性 (Navigation Properties) ---

        /// <summary>
        /// 指向父層
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual ProductCategory? Parent { get; set; }

        /// <summary>
        /// 指向子層列表
        /// </summary>
        public virtual ICollection<ProductCategory> Children { get; set; } = new List<ProductCategory>();
    }
}
