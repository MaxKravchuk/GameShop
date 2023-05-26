﻿using System.Collections.Generic;
using System.Linq;

namespace GameShop.BLL.Pagination.Extensions
{
    public static class IEnumerableExtension
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
