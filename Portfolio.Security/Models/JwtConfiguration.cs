using System;
using System.Collections.Generic;
using System.Text;

namespace BoilerPlate.Security.Models
{
    public class JwtConfiguration
    {
        public string ServerSecret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int ExpiresIn { get; set; }
    }
}
