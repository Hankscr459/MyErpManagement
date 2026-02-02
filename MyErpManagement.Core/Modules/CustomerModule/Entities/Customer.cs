using System.ComponentModel.DataAnnotations;

namespace MyErpManagement.Core.Modules.CustomerModule.Entities
{
    public class Customer
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// 編碼
        /// </summary>
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
        /// 建立日期
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTimeOffset UpdatedAt { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// 餘額
        /// </summary>
        public Decimal Balance { get; set; } = 0;

        /// <summary>
        /// 客戶與標籤的關聯 (中間表)
        /// </summary>
        public virtual ICollection<CustomerTagRelation> CustomerTagRelations { get; set; } = new List<CustomerTagRelation>();
        
        // 如果您希望在程式碼中方便存取，可以加一個唯讀屬性（選配）
        // public IEnumerable<CustomerTag> Tags => CustomerTagRelations.Select(r => r.Tag);
    }
}
