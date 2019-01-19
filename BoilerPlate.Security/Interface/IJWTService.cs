using BoilerPlate.ModelLayer.Identity;
using BoilerPlate.Security.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Security.Interface
{
    public interface  IJWTService
    {
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        Jwt GenerateJwtToken(ApplicationUser user);
        Task<JwtWithRefreshToken> GenerateJWtWithRefreshTokenAsync(ApplicationUser user );
    }
}
