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
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// 餘額
        /// </summary>
        public Decimal Balance { get; set; } = 0;
    }
}
