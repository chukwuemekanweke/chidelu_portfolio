using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBackendBoilerPlate.Infrastructure
{
    public class JwtConfiguration
    {
        public string ServerSecret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int ExpiresIn { get; set; }
    }
}
