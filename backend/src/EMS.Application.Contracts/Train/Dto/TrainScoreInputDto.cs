using Mbp.Ddd.Application.Mbp.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.Train.Dto
{
    public class TrainScoreInputDto : DtoBase
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string LoginName { get; set; }

        public string UserName { get; set; }

        public string Major { get; set; }

        public int SubjectId { get; set; }

        public string SubjectCode { get; set; }

        public string SubjectName { get; set; }

        public DateTime TrainDate { get; set; }

        public decimal TrainHour { get; set; }

        public decimal Score { get; set; }

        public string Remark { get; set; }

        public bool IsDeleted { get; set; }
    }
}
