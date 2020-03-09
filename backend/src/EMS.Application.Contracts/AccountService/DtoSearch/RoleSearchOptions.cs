using Mbp.Ddd.Application.Mbp.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.AccountService.DtoSearch
{
    public class RoleSearchOptions
    {
        public string Name { get; set; } = string.Empty;

        public string SystemCode { get; set; } = string.Empty;
    }
}
