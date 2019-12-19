using System;
using System.Collections.Generic;
using System.Text;

namespace Medical.Ai.Mbdp.Application.Contracts.LogService.DtoSearch
{
    public class LogSearchOptions
    {
        public string UserName { get; set; }

        public string ClientIP { get; set; } = string.Empty;

        public DateTime OpDateTimeBegin { get; set; } = DateTime.Today.AddDays(-3);

        public DateTime OpDateTimeEnd { get; set; } = DateTime.Today.AddDays(1);

        public string AppName { get; set; } = string.Empty;

        public string ModuleName { get; set; } = string.Empty;

        public string OpName { get; set; } = string.Empty;

        public string Desc { get; set; } = string.Empty;
    }
}
