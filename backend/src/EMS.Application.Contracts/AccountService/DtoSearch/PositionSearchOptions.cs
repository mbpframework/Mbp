using Mbp.EntityFrameworkCore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.AccountService.DtoSearch
{
   public class PositionSearchOptions
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string SystemCode { get; set; }

        public EnumPositionType PositionType { get; set; }
    }
}
