using System;
using System.Collections.Generic;
using System.Text;

namespace BoilerPlate.ModelLayer.Entity
{
    public class REFRESH_TOKENS
    {
        public Guid Id { get; set; }
        public string IdentityUserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public string RemoteIpAddress { get; set; }
        public string Longtitude { get; set; }
        public string Latitude { get; set; }
        public string ContinentName { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
    }
}
