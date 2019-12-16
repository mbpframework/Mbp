using Mbp.Ddd.Application.Mbp.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medical.Ai.Mbdp.Application.Contracts.AccountService.Dto
{
    public class LoginInputDto : DtoBase
    {
        public string LoginName { get; set; }

        public string Password { get; set; }

        public string ClientID { get; set; }
    }
}
