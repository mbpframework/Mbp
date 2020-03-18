using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.AspNetCore.Http.Context
{
    public class UserSession
    {
        public int Id { get; set; }

        public string LoginName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string RoleId { get; set; }
    }
}
