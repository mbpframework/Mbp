using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.Train.DtoSearch
{
    public class TrainPlanYearSearchOptions
    {
        public int Year { get; set; }

        public string Title { get; set; }

        public string DeptName { get; set; }
    }
}
