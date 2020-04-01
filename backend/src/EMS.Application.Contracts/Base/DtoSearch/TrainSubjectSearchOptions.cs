using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.Base.DtoSearch
{
    public class TrainSubjectSearchOptions
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string SystemCode { get; set; }

        public int PositionId { get; set; }
    }
}
