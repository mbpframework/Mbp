using Mbp.Ddd.Application.Mbp.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.FileServer.Dto
{
    public class AttachmentOutputDto : DtoBase
    {
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        public string AttachmentTypeElementCode { get; set; }

        public string BussinessTypeElementCode { get; set; }

        public Guid BussinessId { get; set; }
    }
}
