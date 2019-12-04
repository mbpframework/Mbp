using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mbp.Ddd.Application.Mbp.UI
{
    public class PagedList<T>
    {
        /// <summary>
        /// 获取或设置总记录数。
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 获取或设置页面大小。
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 获取或设置页码。
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 获取或设置当前页面的数据。
        /// </summary>
        public List<T> Content { get; set; }
    }
}
