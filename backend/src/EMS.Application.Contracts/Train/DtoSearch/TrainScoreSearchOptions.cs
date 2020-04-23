using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.Train.DtoSearch
{
    public class TrainScoreSearchOptions
    {
        public string UserName { get; set; }

        public string Major { get; set; }

        public int SubjectId { get; set; }

        public DateTime TrainDate { get; set; } = DateTime.MinValue;

        public decimal Score { get; set; }
    }
}
