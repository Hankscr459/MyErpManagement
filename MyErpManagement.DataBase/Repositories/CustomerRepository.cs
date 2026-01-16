using Microsoft.EntityFrameworkCore;
using MyErpManagement.Core.Enums;
using MyErpManagement.Core.Modules.CustomerModule.Entities;
using MyErpManagement.Core.Modules.CustomerModule.Enums;
using MyErpManagement.Core.Modules.CustomerModule.IRepositories;
using MyErpManagement.Core.Modules.ProductsModule.Models;

namespace MyErpManagement.DataBase.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext db) : base(db)
        {
        }
        public IQueryable<Customer> GetSearchQuery(CustomerListQueryModel queryModel)
        {
            // 1. 取得基礎 Queryable (預設不追蹤)
            IQueryable<Customer> query = dbSet.AsNoTracking();

            // 2. 處理關鍵字查詢
            if (!string.IsNullOrWhiteSpace(queryModel.Search))
            {
                string search = queryModel.Search.Trim();

                query = query.Where(c =>
                    c.Name.Contains(search) ||
                    c.Code.Contains(search) ||
                    c.Phone.Contains(search)
                );
            }

            // 3. 處理排序
            query = ApplySorting(query, queryModel);

            return query;
        }

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
    }
}
