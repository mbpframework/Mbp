using System;
using System.Collections.Generic;
using System.Text;

namespace Medical.Ai.Mbdp.Application.AccountService.Dto
{
    public class LoginInputDto
    {
        public string LoginName { get; set; }

        public string Password { get; set; }

        public string ClientID { get; set; }
    }
}
