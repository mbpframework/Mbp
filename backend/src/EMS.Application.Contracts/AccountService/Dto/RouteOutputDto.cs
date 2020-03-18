using Mbp.Ddd.Application.Mbp.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.AccountService.Dto
{
    public class RouteOutputDto : DtoBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        public int ParentId { get; set; }

        public string Code { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("component")]
        public string Component { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public int Order { get; set; }

        [JsonProperty("meta")]
        public RouteMetaOutput Meta { get; set; } = new RouteMetaOutput();

        [JsonProperty("children")]
        public List<RouteOutputDto> Children { get; private set; } = new List<RouteOutputDto>();
    }
}
