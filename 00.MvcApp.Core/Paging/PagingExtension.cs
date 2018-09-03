using System.Collections.Generic;
using System.Linq;

namespace MvcApp.Core.Paging
{
    public static class PagingExtension
    {
        public static PageResult<T> ToPageResult<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            return new PageResult<T>(source, pageIndex, pageSize, source.Count());
        }

        public static PageResult<T> ToPageResult<T>(this IQueryable<T> linq, int pageIndex, int pageSize)
        {
            return new PageResult<T>(linq, pageIndex, pageSize);
        }

        public static PageResult<T> ToPageResult<T>(this IQueryable<T> query, PageRequest request)
        {
            return new PageResult<T>(query.OrderBy(request.Sort, request.SortDirection), request.PageIndex, request.PageSize);
        }

        public static PageResult<T> ToPageResult<T>(this IList<T> source, int pageIndex, int pageSize)
        {
            return new PageResult<T>(source, pageIndex, pageSize);
        }

        public static PageResult<TRank> CalculateRank<TRank>(this PageResult<TRank> result) where TRank : IRankEntity
        {
            var start = result.PageIndex * result.PageSize;
            for (var i = 0; i < result.Value.Count; i++)
            {
                result.Value[i].Rank = start + i + 1;
            }
            return result;
        }

        public static List<TRank> CalculateRank<TRank>(this List<TRank> result) where TRank : IRankEntity
        {
            var start = 0;
            for (var i = 0; i < result.Count; i++)
            {
                result[i].Rank = start + i + 1;
            }
            return result;
        }
    }
}
