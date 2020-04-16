using Mbp.Ddd.Application.Mbp.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.Train.Dto
{
    public class TrainPlanWeekDetailOutputDto : DtoBase
    {
        public int Id { get; set; }

        public int EmsTrainPlanWeekId { get; set; }

        /// <summary>
        /// 训练日期
        /// </summary>
        public DateTime TrainDate { get; set; }

        /// <summary>
        /// 星期几
        /// </summary>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// 上下午时间段
        /// </summary>
        public string AmPm { get; set; }

        /// <summary>
        /// 训练内容,字符串
        /// </summary>
        public string SubjectContent { get; set; }

        /// <summary>
        /// 参训对象,字符串
        /// </summary>
        public string AttendOject { get; set; }

        /// <summary>
        /// 组训方法,集中组织,统一组织,自训
        /// </summary>
        public string TrainMethod { get; set; }

        /// <summary>
        /// 组织者
        /// </summary>
        public string Organizer { get; set; }

        /// <summary>
        /// 质量指标
        /// </summary>
        public string QualityIndex { get; set; }

        /// <summary>
        /// 保障措施
        /// </summary>
        public string Safeguards { get; set; }

        /// <summary>
        /// 课时
        /// </summary>
        public decimal TrainHour { get; set; }

        /// <summary>
        /// 地点
        /// </summary>
        public string Address { get; set; }
    }
}
