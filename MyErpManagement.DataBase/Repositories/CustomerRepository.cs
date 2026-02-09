using Microsoft.EntityFrameworkCore;
using MyErpManagement.Core.Enums;
using MyErpManagement.Core.Modules.CustomerModule.Entities;
using MyErpManagement.Core.Modules.CustomerModule.Enums;
using MyErpManagement.Core.Modules.CustomerModule.IRepositories;
using MyErpManagement.Core.Modules.CustomerModule.Models;

namespace MyErpManagement.DataBase.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext db) : base(db)
        {
        }

        public IQueryable<Customer> GetSearchQuery(CustomerListQueryModel queryModel)
        {
            // 取得基礎 Queryable (預設不追蹤)
            IQueryable<Customer> query = dbSet.AsNoTracking();

            // 關鍵字過濾：比對名稱、編號、電話
            if (!string.IsNullOrWhiteSpace(queryModel.Search))
            {
                string search = queryModel.Search.Trim();

                query = query.Where(c =>
                    c.Name.Contains(search) ||
                    c.Code.Contains(search) ||
                    c.Phone.Contains(search)
                );
            }

            // 處理標籤篩選 (聯集查詢：只要符合其中一個標籤就抓出來)
            if (queryModel.CustomerTagIds is not null && queryModel.CustomerTagIds.Any())
            {
                // 取得所有欲查詢的標籤 ID
                var filterTagIds = queryModel.CustomerTagIds;

                // 篩選：客戶擁有的標籤關聯中，只要有「任一筆」的標籤 ID 存在於 filterTagIds 中即可
                query = query.Where(c => c.CustomerTagRelations.Any(ctr => filterTagIds.Contains(ctr.CusomterTagId)));
            }

            // 處理排序
            query = ApplySorting(query, queryModel);

            return query;
        } 

        /// <summary>
        /// 封裝排序邏輯，根據模型指定的欄位與方向進行排序
        /// </summary>
        /// <param name="query">原始查詢物件</param>
        /// <param name="queryModel">包含排序資訊的模型</param>
        /// <returns>帶有排序條件的查詢物件</returns>
        private IQueryable<Customer> ApplySorting(IQueryable<Customer> query, CustomerListQueryModel queryModel)
        {
            bool isDesc = queryModel.SortDir == SortDirEnum.desc;

            // 根據 Enum 切換排序欄位
            return queryModel.SortBy switch
            {
                CustomerListSortByEnum.name => isDesc ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                CustomerListSortByEnum.code => isDesc ? query.OrderByDescending(c => c.Code) : query.OrderBy(c => c.Code),
                CustomerListSortByEnum.updatedAt => isDesc ? query.OrderByDescending(c => c.UpdatedAt) : query.OrderBy(c => c.UpdatedAt),
                
                // 預設以建立時間排序
                _ => isDesc ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt)
            };
        }

        public async Task UpdateCustomerTags(Guid customerId, List<Guid>? newTagIds)
        {
            if (newTagIds is null) return;
            if (newTagIds.Count == 0) return;

            // 撈出目前的關聯 (不使用 AsNoTracking，因為要更新)
            var customer = await dbSet
                .Include(c => c.CustomerTagRelations)
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer is null) return;

            // 執行「刪除」邏輯：找出 [舊的有，新的沒有] 的關聯
            var tagsToRemove = customer.CustomerTagRelations
                .Where(ctr => !newTagIds.Contains(ctr.CusomterTagId))
                .ToList();

            foreach (var tag in tagsToRemove)
            {
                // 從導覽屬性移除，EF 會在 SaveChanges 時標記為 Deleted
                customer.CustomerTagRelations.Remove(tag);
            }

            /// 執行「新增」邏輯：找出 [新的有，舊的沒有] 的標籤 ID
            var existingTagIds = customer.CustomerTagRelations.Select(ctr => ctr.CusomterTagId).ToList();
            var tagsToAdd = newTagIds.Except(existingTagIds);
            foreach (var tagId in tagsToAdd)
            {
                customer.CustomerTagRelations.Add(new CustomerTagRelation
                {
                    CustomerId = customerId,
                    CusomterTagId = tagId,
                    AssignedAt = DateTime.UtcNow,
                });
            }
            // 注意：此處僅修改追蹤實體狀態，最後需透過 UnitOfWork 或 DbContext.SaveChangesAsync() 寫入資料庫
        }
    }
}
