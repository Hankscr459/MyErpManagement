using System.ComponentModel.DataAnnotations;

namespace MyErpManagement.Core.Dtos.Suppliers.Request
{
    public class CreateSupplierRequestDto
    {
        /// <summary>
        /// 名稱
        /// </summary>
        [Required]
        public string Name { get; set; } = default!;

        /// <summary>
        /// 編碼
        /// </summary>
        [Required]
        public string Code { get; set; } = default!;

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; } = default!;

        /// <summary>
        /// 手機號碼
        /// </summary>
        [RegularExpression(@"^\d{10,}$")]
        public string Phone { get; set; } = default!;

        /// <summary>
        /// 備註
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// 供應商標籤Id列表
        /// </summary>
        public List<Guid>? SupplierTagIds { get; set; }
    }
}
