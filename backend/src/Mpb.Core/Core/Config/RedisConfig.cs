using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Core.Core.Config
{
    public class RedisConfig
    {
        public List<string> EndPoints { get; set; }

        public string Password { get; set; }

        public int ConnectTimeout { get; set; }

        public string ClientName { get; set; }

        public int KeepAlive { get; set; }

        public string DefaultVersion { get; set; }
    }
}
