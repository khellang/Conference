using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Conference
{
    public static class QueryableExtensions
    {
        public static async Task<PageModel<T>> PagedAsync<T>(this IQueryable<T> source, PagedQuery query, CancellationToken cancellationToken)
            where T : class
        {
            var count = await source.LongCountAsync(cancellationToken);

            var page = query.Page ?? 1;
            var size = query.Size ?? 25;

            var pageIndex = Math.Max(page - 1, 0);
            var pageSize = Math.Clamp(size, min: 1, max: 200);

            var items = await source
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PageModel<T>(items, count, pageIndex, pageSize);
        }
    }
}
