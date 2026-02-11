using MyErpManagement.Core.Modules.OrderNoModule.Entities;
using MyErpManagement.Core.Modules.OrderNoModule.Enums;
using MyErpManagement.Core.Modules.OrderNoModule.IRepositories;
using MyErpManagement.Core.Modules.OrderNoModule.IServices;
using System.Security.Cryptography;
using System.Text;

namespace MyErpManagement.Core.Modules.OrderNoModule.Services
{
    /// <summary>
    /// 訂單編號產生服務
    /// 負責產生：
    /// 1. 私有訂單編號（依據訂單類型 + 年月流水號）
    /// 2. 公開訂單編號（日期 + 隨機字串）
    /// </summary>
    public class OrderNoService(IOrderSequenceRepository orderSequenceRepository) : IOrderNoService
    {
        private const string Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public async Task<string> GeneratePrivateOrderNo(OrderTypeEnum orderType, DateTime date)
        {
            // 依據日期取得期別（yyyyMM），作為每月流水號依據
            var period = date.ToString("yyyyMM");

            // 查詢是否已存在該訂單類型 + 期別 的流水號資料
            var orderSequence = await orderSequenceRepository.GetFirstOrDefaultAsync(
                os => os.OrderType == orderType.ToString() && os.Period == period,
                null, true
            );
            if (orderSequence is null)
            {
                // 若該期別尚未建立，初始化流水號為 1
                orderSequence = new OrderSequence
                {
                    Period = period,
                    OrderType = orderType.ToString(),
                    CurrentNo = 1,
                };
                orderSequenceRepository.Add(orderSequence);
            }
            else
            {
                // 若已存在，流水號 +1
                orderSequence.CurrentNo++;
                orderSequenceRepository.Update(orderSequence);
            }

            // 將流水號補滿 6 位數（例如 1 -> 000001）
            var runningNo = orderSequence.CurrentNo.ToString("D6");
            return $"{orderType}-{date.ToString("yyyyMMdd")}-{runningNo}";
        }

        public string GeneratePublicOrderNo(DateTime date)
        {
            var datePart = date.ToString("yyyyMMdd");

            var randomPart = GenerateRandomString(8);

            return datePart + randomPart;
        }

        /// <summary>
        /// 產生指定長度的隨機英數字字串
        /// 使用 RandomNumberGenerator（加密等級亂數）
        /// </summary>
        /// <param name="length">字串長度</param>
        /// <returns>隨機英數字字串</returns>
        private static string GenerateRandomString(int length)
        {
            var result = new StringBuilder();
            // 建立緩衝區
            var buffer = new byte[length];

            // 使用加密安全亂數產生器
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(buffer);

            // 將每個 byte 映射到 Chars 字元集合
            for (int i = 0; i < length; i++)
            {
                result.Append(Chars[buffer[i] % Chars.Length]);
            }

            return result.ToString();
        }
    }
}
