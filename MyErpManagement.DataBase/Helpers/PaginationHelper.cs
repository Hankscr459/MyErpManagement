using Microsoft.EntityFrameworkCore;
using MyErpManagement.Core.Models;

namespace MyErpManagement.DataBase.Helpers
{
    public static class PaginationHelper
    {
        public static async Task<PaginatedResultModel<T>> ToPagedListAsync<T>(
            this IQueryable<T> query, int page = 1, int limit = 10)
        {
            page = page < 1 ? 1 : page;
            limit = limit < 1 ? 10 : limit;

            var totalDocs = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalDocs / (double)limit);

            var docs = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return new PaginatedResultModel<T>
            {
                Docs = docs,
                TotalDocs = totalDocs,
                Limit = limit,
                Page = page,
                TotalPages = totalPages,
                HasPrevPage = page > 1,
                HasNextPage = page < totalPages,
                PrevPage = page > 1 ? page - 1 : null,
                NextPage = page < totalPages ? page + 1 : null,
                PagingCounter = ((page - 1) * limit) + 1
            };
        }
    }
}
