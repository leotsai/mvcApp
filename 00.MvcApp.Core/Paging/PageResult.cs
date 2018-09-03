using System.Collections.Generic;
using System.Linq;

namespace MvcApp.Core.Paging
{
    public class PageResult
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage => (PageIndex > 0);

        public bool HasNextPage => (PageIndex + 1 < TotalPages);

        public void UpdateFrom(PageResult result)
        {
            this.PageIndex = result.PageIndex;
            this.PageSize = result.PageSize;
            this.TotalCount = result.TotalCount;
            this.TotalPages = result.TotalPages;
        }

        public PageResult()
        {

        }

        public PageResult(PageRequest request, int total)
        {
            this.PageIndex = request.PageIndex;
            this.PageSize = request.PageSize;
            this.TotalCount = total;

            this.TotalPages = total / this.PageSize;
            if (total % this.PageSize > 0)
            {
                this.TotalPages++;
            }
        }
    }

    public class PageResult<T> : PageResult
    {
        public List<T> Value { get; set; }
        
        public PageResult()
        {
            this.Value = new List<T>();
        }

        public PageResult(List<T> value, PageRequest request, int total) : base(request, total)
        {
            this.Value = value;
        }

        public PageResult(IQueryable<T> source, int pageIndex, int pageSize)
            : this()
        {
            if (pageSize == 0)
            {
                return;
            }
            int total = source.Count();
            this.TotalCount = total;
            this.TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.Value.AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        public PageResult(IList<T> source, int pageIndex, int pageSize)
            : this()
        {
            if (pageSize == 0)
            {
                return;
            }
            TotalCount = source.Count;
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.Value.AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        public PageResult(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
            : this()
        {
            if (pageSize == 0)
            {
                return;
            }
            TotalCount = totalCount;
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.Value.AddRange(source.Skip(pageIndex * pageSize).Take(pageSize));
        }

        public PageResult<T> Fill(List<T> value, PageRequest request, int total)
        {
            this.Value = value;
            this.PageIndex = request.PageIndex;
            this.PageSize = request.PageSize;
            this.TotalCount = total;
            this.TotalPages = this.TotalCount / this.PageSize;
            if (TotalCount%this.PageSize > 0)
            {
                this.TotalPages++;
            }
            return this;
        } 
    }
}
