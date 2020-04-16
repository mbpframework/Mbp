using EMS.Domain.DomainEntities.Base;
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

        public EnumTrainType TrainType { get; set; }
    }
}
