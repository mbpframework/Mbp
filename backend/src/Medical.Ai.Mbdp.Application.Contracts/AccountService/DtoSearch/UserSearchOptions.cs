using System;
using System.Collections.Generic;
using System.Text;

namespace Medical.Ai.Mbdp.Application.Contracts.AccountService.DtoSearch
{
    public class UserSearchOptions
    {
        public string UserName { get; set; } = string.Empty;

        public string LoginName { get; set; } = string.Empty;

        public string IsAdmin { get; set; } = string.Empty;
    }
}
