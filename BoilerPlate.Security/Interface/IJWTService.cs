using BoilerPlate.ModelLayer.Identity;
using BoilerPlate.Security.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace BoilerPlate.Security.Interface
{
    public interface  IJWTService
    {
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        Jwt GenerateJWTToken(ApplicationUser user);
       
    }
}
