using MyErpManagement.Core.Models;
using System.Linq.Expressions;

namespace MyErpManagement.Core.IRepositories
{
    public interface IRepository<T> where T : class
    {

        #region 新增方法 (Create)

        void Add(T entity);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        #endregion 新增方法 (Create)


        #region 讀取方法 (Read)
        /// <summary>
        /// 通過條件查詢數據
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter">查詢條件</param>
        /// <param name="selector">返回類型如:Find(x => x.UserName == loginInfo.userName, p => new { uname = p.UserName });</param>
        /// <returns></returns>
        Task<List<TEntity>> FindAllByFilterAndSelectAsync<TEntity>(Expression<Func<T, bool>> filter, Expression<Func<T, TEntity>> selector);
        IQueryable<TEntity> FilterListQuery<TEntity>(Expression<Func<T, bool>> filter, Expression<Func<T, TEntity>> selector);

        #endregion 讀取方法 (Read)


        #region 修改方法 (Update)

        void Update(T entity);

        #endregion 修改方法 (Update)

        #region 讀取同時也可以新增方法 (Read & Update)

        /// <summary>
        /// 根據過濾條件取得單一實體，並可選擇性地加載關聯資料與設定是否追蹤。
        /// </summary>
        /// <param name="filter">Lambda 表達式，定義查詢條件 (WHERE 子句)。</param>
        /// <param name="includeProperties">要預先加載的關聯屬性名稱，多個屬性請用逗號分隔，例如 "Department,Roles"。</param>
        /// <param name="tracked">是否由 EF Context 追蹤此實體。預設為 false (唯讀模式，效能較好)。</param>
        /// <returns>回傳找到的第一筆實體；若無符合條件者則回傳 null。</returns>
        Task<IEnumerable<T>> FindAllByFilterOrUpdate(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T?> FindOneByFilterOrUpdate(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);

        #endregion 讀取同時也可以新增方法 (Read & Update)


        #region 移除方法 (Read)

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
        void RemoveByIdList(IEnumerable<Guid> ids);
        
        #endregion 移除方法 (Delete)
    }
}
