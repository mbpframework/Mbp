using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Ddd.Application.Mbp.Dto
{
    public interface IHasAttachmentDto
    {
        Guid AttachmentRelative { get; set; }

        string AttachmentName { get; set; }

        string AttachementUrl { get; set; }
    }
}
