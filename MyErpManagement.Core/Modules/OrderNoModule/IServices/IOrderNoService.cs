using MyErpManagement.Core.Modules.OrderNoModule.Enums;

namespace MyErpManagement.Core.Modules.OrderNoModule.IServices
{
    public interface IOrderNoService
    {
        /// <summary>
        /// 產生私有訂單編號
        /// 格式：{OrderType}-{yyyyMMdd}-{6位數流水號}
        /// 
        /// 規則：
        /// - 每個 OrderType 每月（yyyyMM）獨立計算流水號
        /// - 每個月從 000001 開始累加
        /// </summary>
        /// <param name="orderType">訂單類型</param>
        /// <param name="date">訂單日期（決定所屬年月與日期字串）</param>
        /// <returns>組合完成的私有訂單編號</returns>
        Task<string> GeneratePrivateOrderNo(OrderTypeEnum orderType, DateTime date);
        
        /// <summary>
        /// 產生公開訂單編號
        /// 格式：yyyyMMdd + 8碼隨機英數字
        /// 
        /// 特點：
        /// - 不依賴資料庫
        /// - 使用加密等級亂數產生器
        /// - 適用於對外顯示或公開識別碼
        /// </summary>
        /// <param name="date">訂單日期</param>
        /// <returns>公開訂單編號</returns>
        string GeneratePublicOrderNo(DateTime date);
    }
}
