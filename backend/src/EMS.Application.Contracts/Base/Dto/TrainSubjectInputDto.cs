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
        /// 岗位Id
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 科目编码
        /// </summary>
        public string SubjectCode { get; set; }

        public byte[] ConcurrencyStamp { get; set; }
    }
}
