namespace MyErpManagement.Core.Modules.InventoryModule.Models
{
    /// <summary>
    /// 調貨新增庫存物件
    /// </summary>
    public class TransferInventoryModel
    {
        public Guid ProductId { get; set; }
        /// <summary>
        /// 出庫倉庫Id
        /// </summary>
        public Guid FromWareHouseId { get; set; }
        /// <summary>
        /// 入庫倉庫Id
        /// </summary>
        public Guid ToWareHouseId { get; set; }
        /// <summary>
        /// 調貨數量
        /// </summary>
        public int Quantity { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
