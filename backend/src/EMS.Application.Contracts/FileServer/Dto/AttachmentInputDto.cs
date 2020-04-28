using Mbp.Ddd.Application.Mbp.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.FileServer.Dto
{
    public class AttachmentInputDto : DtoBase
    {
        public int Id { get; set; }

        public string AttachmentTypeElementCode { get; set; }

        public string BussinessTypeElementCode { get; set; }

        public Guid BussinessId { get; set; }
    }
}
