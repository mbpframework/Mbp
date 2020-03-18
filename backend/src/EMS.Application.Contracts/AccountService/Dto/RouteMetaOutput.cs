using Mbp.Ddd.Application.Mbp.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.AccountService.Dto
{
    public class RouteMetaOutput : DtoBase
    {
        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
