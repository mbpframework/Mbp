using Mbp.Core.Entity;
using Mbp.Core.Entity.Aggregate;
using Mbp.EntityFrameworkCore.PermissionModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.Domain.DomainEntities.Train.Plan
{
    /// <summary>
    /// 训练计划
    /// </summary>
    public class EmsTrainPlanWeek : AggregateBase<int>, ISoftDelete
    {
        /// <summary>
        /// 部门Id
        /// </summary>
        public int DeptId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间 一般为开始时间+5
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 第几周
        /// </summary>
        public int WeekNum { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public short Month { get; set; }

        /// <summary>
        /// 训练详情
        /// </summary>
        public List<EmsTrainPlanWeekDetail> TrainPlanWeekDetails { get; set; }

        public bool IsDeleted { get; set; }
    }
}
