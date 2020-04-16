using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.Train.DtoSearch
{
    public class TrainPlanWeekSearchOptions
    {
        public string Title { get; set; }

        public string DeptName { get; set; }

        public DateTime BeginTime { get; set; } = DateTime.MinValue;

        public DateTime EndTime { get; set; } = DateTime.MaxValue;
    }
}
