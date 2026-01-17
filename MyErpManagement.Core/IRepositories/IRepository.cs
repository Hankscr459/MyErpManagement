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
        /// 根據條件過濾並選取特定欄位（非同步），預設不追蹤 (No-Tracking)。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter">過濾條件</param>
        /// <param name="selector">選取欄位表達式 返回類型如:Find(x => x.UserName == loginInfo.userName, p => new { uname = p.UserName });</param>
        /// <returns></returns>
        Task<List<TEntity>> ProjectAsync<TEntity>(Expression<Func<T, bool>> filter, Expression<Func<T, TEntity>> selector);
        
        /// <summary>
        /// 建立一個帶有條件與選擇器的查詢 (IQueryable)，預設不追蹤。
        /// 用於延遲執行 (Deferred Execution)。
        /// </summary>
        IQueryable<TEntity> ProjectTo<TEntity>(Expression<Func<T, bool>> filter, Expression<Func<T, TEntity>> selector);
        
        /// <summary>
        /// 快速過濾查詢（預設不追蹤）。
        /// </summary>
        IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate);

        #endregion 讀取方法 (Read)


        #region 修改方法 (Update)

        /// <summary>
        /// 更新實體狀態。
        /// 將實體標記為 Modified，調用 SaveChanges 時會更新所有欄位。
        /// </summary>
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
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

        /// <summary>
        /// 取得符合條件的第一筆實體
        /// </summary>
        /// <param name="filter">過濾條件</param>
        /// <param name="includeProperties">關聯載入屬性 (以逗號分隔)</param>
        /// <param name="tracked">是否追蹤實體 (若後續要呼叫 Update，請設為 true)</param>
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);

        #endregion 讀取同時也可以新增方法 (Read & Update)


        #region 移除方法 (Remove)

        /// <summary>
        /// 移除單筆實體（標記為 Deleted）。
        /// </summary>
        void Remove(T entity);

        /// <summary>
        /// 批次移除多筆實體。
        /// </summary>
        void RemoveRange(IEnumerable<T> entity);

        /// <summary>
        /// 根據 ID 列表直接從資料庫刪除 (EF Core 7+ 支援)。
        /// 注意：此方法會立即執行 SQL，不需調用 SaveChanges()，且不會觸發 ChangeTracker 追蹤。
        /// </summary>
        /// <param name="ids">Guid 型態的 ID 集合</param>
        void RemoveByIdList(IEnumerable<Guid> ids);
        
        #endregion 移除方法 (Delete)
    }
}
