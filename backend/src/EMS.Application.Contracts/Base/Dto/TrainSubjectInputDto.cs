using EMS.Domain.DomainEntities.Base;
using Mbp.Ddd.Application.Mbp.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.Base.Dto
{
    public class TrainSubjectInputDto : DtoBase
    {
        public int Id { get; set; }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 科目编码
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// 训练类型
        /// </summary>
        public EnumTrainType TrainType { get; set; }

        /// <summary>
        /// 训练课时(小时)
        /// </summary>
        public decimal TrainHour { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        public byte[] ConcurrencyStamp { get; set; }
    }
}
