using Microsoft.EntityFrameworkCore;
using MyErpManagement.Core.IRepositories;
using System.Linq.Expressions;

namespace MyErpManagement.DataBase.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        #region 寫入 (Write)
        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public virtual Task AddRangeAsync(IEnumerable<T> entities)
        {
            return dbSet.AddRangeAsync(entities);
        }
        #endregion


        #region 修改 (Update)

        public void Update(T entity)
        {
            dbSet.Entry(entity).State = EntityState.Modified;
        }

        #endregion 修改 (Update)


        #region 讀取方法 (Read)


        public virtual async Task<List<TEntity>> ProjectAsync<TEntity>(Expression<Func<T, bool>> filter, Expression<Func<T, TEntity>> selector)
        {
            return await dbSet.Where(filter).AsNoTracking().Select(selector).ToListAsync();
        }

        public virtual IQueryable<TEntity> ProjectTo<TEntity>(Expression<Func<T, bool>> filter, Expression<Func<T, TEntity>> selector)
        {
            return dbSet.Where(filter).AsNoTracking().Select(selector);
        }

        #endregion


        #region 讀取 & 修改 方法 (Read & Udate)
        protected IQueryable<T> BuildQuery(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null,
            bool tracked = false
        )
        {
            // 1. 設定追蹤模式 (預設不追蹤以提高效能)
            IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();

            // 2. 套用過濾條件
            if (filter is not null)
            {
                query = query.Where(filter);
            }

            // 3. 套用 Eager Loading (關聯載入)
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }

            return query;
        }
        public virtual async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            // 調用封裝好的查詢建構器
            IQueryable<T> query = BuildQuery(filter, includeProperties, tracked);
            return await query.FirstOrDefaultAsync();

        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate)
        {
            return dbSet.AsNoTracking().Where(predicate);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter, string? includeProperties = null)
        {
            return await BuildQuery(filter, includeProperties).ToListAsync();
        }

        #endregion


        #region 刪除 (Delete)

        public virtual void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }

        public void RemoveByIdList(IEnumerable<Guid> ids)
        {
            var entitiesToRemove = dbSet
            .Where(e => ids.Contains(EF.Property<Guid>(e, "Id")))
            .ToList(); // 先載入實體

                dbSet.RemoveRange(entitiesToRemove);
        }

        #endregion
    }
}
