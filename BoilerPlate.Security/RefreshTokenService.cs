using BoilerPlate.Security.Interface;
using BoilerPlate.Security.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Security
{
    public class RefreshTokenService:IRefreshTokenService, ISecurityBaseService
    {
        ISecurityServiceFactory SecurityServiceFactory { get; set; }
        public RefreshTokenService(ISecurityServiceFactory securityServiceFactory)
        {
            SecurityServiceFactory = securityServiceFactory;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public string GenerateAndSaveRefreshToken(string identityUserId)
        {
            string refreshToken = GenerateRefreshToken();
            SaveRefreshToken(identityUserId, refreshToken);
            return refreshToken;
        }

        public  async Task<JwtWithRefreshToken> Refresh(string token, string refreshToken)
        {

            IJWTService jwtService = SecurityServiceFactory.GetService(typeof(IJWTService)) as IJWTService;
            IIdentityUserService identityUserService = SecurityServiceFactory.GetService(typeof(IIdentityUserService)) as IIdentityUserService;

            var principal = jwtService.GetPrincipalFromExpiredToken(token);
            var username = principal.Identity.Name;

            var identityUserId = principal.Claims.Single(x => x.Type == "nameidentifier").Value;
            var applicationUser = await identityUserService.GetApplicationUserAsync(identityUserId);

            var savedRefreshToken = GetRefreshToken(identityUserId); //retrieve the refresh token from a data store


            if (savedRefreshToken != refreshToken)
                throw new SecurityTokenException("Invalid refresh token");

            var newJwtToken = jwtService.GenerateJWTToken(applicationUser);
            var newRefreshToken = GenerateRefreshToken();
            DeleteRefreshToken(username, refreshToken);
            SaveRefreshToken(username, newRefreshToken);
            return new JwtWithRefreshToken {
                AccessToken = newJwtToken.AccessToken,
                Expires = newJwtToken.Expires,
                Issued = newJwtToken.Issued,
                RefreshToken = newRefreshToken,
            };
        }


        public string GetRefreshToken(string identityUserId)
        {
            return "";
        }

        public void DeleteRefreshToken(string identityUserId, string refreshToken)
        {

        }

        public void SaveRefreshToken(string identityUserId, string refreshToken)
        {

        }



    }
}
