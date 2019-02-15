using BoilerPlate.Security.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Security.Interface
{
    public interface  IRefreshTokenService
    {
        Task<JwtWithRefreshToken> Refresh(string token, string refreshToken);
        string GenerateAndSaveRefreshToken(string identityUserId);
    }
}
