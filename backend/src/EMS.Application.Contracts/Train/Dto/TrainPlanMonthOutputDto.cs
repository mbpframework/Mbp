using Mbp.Ddd.Application.Mbp.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.Train.Dto
{
    public class TrainPlanMonthOutputDto : DtoBase
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
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public string Month { get; set; }


        public bool IsDeleted { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public Guid AttachmentRelative { get; set; }

        public byte[] ConcurrencyStamp { get; set; }
    }
}
