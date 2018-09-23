using System;
using System.Collections.Generic;
using System.Text;

namespace AdocicaMel.Core.Domain.Pagination
{
    public class PagedResult<TEntity> where TEntity : class
    {
        private readonly IEnumerable<TEntity> _items;
        private readonly long _totalCount;

        public PagedResult(IEnumerable<TEntity> items, long totalCount)
        {
            _items = items;
            _totalCount = totalCount;
        }

        public IEnumerable<TEntity> Items => _items;
        public long TotalCount => _totalCount;
    }
}
