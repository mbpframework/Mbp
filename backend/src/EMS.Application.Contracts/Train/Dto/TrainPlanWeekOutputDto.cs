using Mbp.Ddd.Application.Mbp.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.Train.Dto
{
    public class TrainPlanWeekOutputDto : DtoBase
    {
        public int Id { get; set; }

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
        public List<TrainPlanWeekDetailOutputDto> TrainPlanWeekDetails { get; set; }

        public byte[] ConcurrencyStamp { get; set; }
    }
}
