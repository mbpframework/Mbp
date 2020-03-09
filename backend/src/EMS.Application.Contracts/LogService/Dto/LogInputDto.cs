using Mbp.Ddd.Application.Mbp.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.LogService.Dto
{
    public class LogInputDto : DtoBase
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }

        public string ClientIP { get; set; }

        public DateTime OpDateTime { get; set; }

        public string AppName { get; set; }

        public string ModuleName { get; set; }

        public string OpName { get; set; }

        public string Desc { get; set; }
    }
}
