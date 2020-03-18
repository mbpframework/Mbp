using Mbp.Core.Entity;
using Mbp.Core.Entity.Aggregate;
using Mbp.EntityFrameworkCore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.EntityFrameworkCore.Domain
{
    public class MbpCategory : AggregateBase<int>, ISoftDelete
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 分类编码
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ParentCategoryName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ParentCategoryCode { get; set; }

        /// <summary>
        /// 分类类型
        /// </summary>
        public EnumCategoryType CategoryType { get; set; }

        public bool IsDeleted { get; set; }

        public string SystemCode { get; set; }
    }
}
