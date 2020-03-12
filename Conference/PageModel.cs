using System;
using System.Collections.Generic;

namespace Conference
{
    public class PageModel<T>
    {
        public PageModel(IReadOnlyCollection<T> items, long totalCount, int pageIndex, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageIndex + 1;
            PageSize = pageSize;
            PageCount = (int) Math.Ceiling((double) totalCount / pageSize);
        }

        public IReadOnlyCollection<T> Items { get; }

        public long TotalCount { get; }

        public int PageNumber { get; }

        public int PageSize { get; }

        public int PageCount { get; }

        public int ItemCount => Items.Count;

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < PageCount;
    }
}