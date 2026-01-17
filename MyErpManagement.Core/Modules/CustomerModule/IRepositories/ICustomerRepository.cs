using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.CustomerModule.Entities;
using MyErpManagement.Core.Modules.ProductsModule.Models;

namespace MyErpManagement.Core.Modules.CustomerModule.IRepositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        /// <summary>
        /// 根據查詢模型建構 IQueryable 查詢物件 (延遲執行)
        /// </summary>
        /// <param name="queryModel">包含搜尋關鍵字、標籤篩選、排序等條件的模型</param>
        /// <returns>回傳尚未執行的 IQueryable 物件</returns>
        IQueryable<Customer> GetSearchQuery(CustomerListQueryModel queryModel);

        /// <summary>
        /// 更新客戶的標籤關聯 (比對差異並執行增刪)
        /// </summary>
        /// <param name="customerId">客戶主鍵 ID</param>
        /// <param name="newTagIds">新設定的標籤 ID 列表</param>
        /// <remarks>
        /// 此方法採用「比對差異 (Diff)」演算法：
        /// 1. 刪除不在新清單中的舊關聯
        /// 2. 新增不在舊關聯中的新標籤
        /// </remarks>
        Task UpdateCustomerTags(Guid customerId, List<Guid>? newTagIds);
    }
}
