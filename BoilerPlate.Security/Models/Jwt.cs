using System;
using System.Collections.Generic;
using System.Text;

namespace BoilerPlate.Security.Models
{
    public class Jwt
    {
        public string AccessToken { get; set; }
        public DateTime Issued { get; set; }
        public DateTime Expires { get; set; }
    }

    public class JwtWithRefreshToken:Jwt
    {
       public string RefreshToken { get; set; }
    }
}
