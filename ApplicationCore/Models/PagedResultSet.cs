using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class PagedResultSet<T> where T : class
    {
        public int PageIndex { get; set; }  // 当前页码（从 1 开始）
        public int PageSize { get; set; }   // 每页显示多少条数据
        public int TotalPages { get; set; } // 总页数 = Count / PageSize
        public long Count { get; set; }     // 总记录数（数据库中的总行数）

        public bool HasPreviousPage => PageIndex > 1;      // 是否有上一页，已经是第一页了，就灰灰掉前一页按钮
        public bool HasNextPage => PageIndex < TotalPages; // 是否有下一页，已经是最后一页了，就灰灰掉下一页按钮

        public IEnumerable<T> Data { get; set; }           // 当前页的数据列表

        public PagedResultSet(IEnumerable<T> data, int pageIndex, int pageSize, long count)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize); // Ceiling() 向上取整
        }
    }
}
