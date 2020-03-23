using Mbp.Core.Entity;
using Mbp.Core.Entity.Aggregate;
using Mbp.EntityFrameworkCore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mbp.EntityFrameworkCore.Domain
{
    public class MbpPosition : AggregateBase<int>, ISoftDelete
    {
        /// <summary>
        /// 岗位名称
        /// </summary>
        [MaxLength(256)]
        public string PositionName { get; set; }

        /// <summary>
        /// 岗位全称
        /// </summary>
        [MaxLength(1024)]
        public string FullPositionName { get; set; }

        /// <summary>
        /// 岗位编码
        /// </summary>
        [MaxLength(256)]
        public string PositionCode { get; set; }

        /// <summary>
        /// 岗位类别
        /// </summary>
        public EnumPositionType PositionType { get; set; }

        /// <summary>
        /// 上级岗位名称
        /// </summary>
        [MaxLength(256)]
        public string ParentPositionName { get; set; }

        /// <summary>
        /// 上级岗位编号
        /// </summary>
        [MaxLength(256)]
        public string ParentPositionCode { get; set; }

        /// <summary>
        /// 上级部门Id
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 上级岗位
        /// </summary>
        [ForeignKey("ParentId")]
        public MbpPosition ParentPosition { get; set; }

        /// <summary>
        /// 下级岗位
        /// </summary>
        public List<MbpPosition> ChildrenPosition { get; set; } = new List<MbpPosition>();

        /// <summary>
        /// 岗位状态
        /// </summary>
        public EnumPositionStatus PositionStatus { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        public bool IsDeleted { get; set; }

        [MaxLength(128)]
        public string SystemCode { get; set; }
    }
}
