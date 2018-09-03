namespace MvcApp.Core.Paging
{
    public class PageRequest
    {
        private int _pageSize;

        public int PageIndex { get; set; }

        public int PageSize
        {
            get { return _pageSize ; }
            set
            {
                if (value > 1000)
                {
                    _pageSize = 100;
                }
                else
                {
                    _pageSize = value;
                }
            }
        }

        public string Sort { get; set; }
        public SortDirection SortDirection { get; set; }
        public int? TotalCount { get; set; }

        public PageRequest()
        {
            
        }

        public PageRequest(int pageIndex, int pageSize)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        public string GetSortSql()
        {
            if (this.SortDirection == SortDirection.None)
            {
                return string.Empty;
            }
            if (this.Sort.Contains("("))
            {
                throw new KnownException("sql system unknown error");
            }
            return $"order by {Sort.Replace(" ", "")} " + (this.SortDirection == SortDirection.Asc ? "asc" : "desc");
        }
        
        public string SqlPaging(string sqlInner)
        {
            var withRowNumber = $"select row_number() over({this.GetSortSql()}) as RowNumber, t0.* from ({sqlInner}) as t0";
            var sql = $"select top {this.PageSize} * from ({withRowNumber}) as t ";
            if (this.PageIndex > 0)
            {
                sql += " where RowNumber > " + this.PageIndex*this.PageSize;
            }
            return sql;
        }
    }
}
