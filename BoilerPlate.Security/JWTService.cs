using BoilerPlate.ModelLayer.Identity;
using BoilerPlate.Security.Interface;
using BoilerPlate.Security.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Security
{
    public class JWTService: ISecurityBaseService
    {

        readonly JwtConfiguration jwtConfig;
        ISecurityServiceFactory SecurityServiceFactory { get; set; }

        public JWTService(ISecurityServiceFactory securityServiceFactory, JwtConfiguration _jwtConfig)
        {
            jwtConfig = _jwtConfig;
            SecurityServiceFactory = securityServiceFactory;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.ServerSecret));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public async Task<JwtWithRefreshToken> GenerateJWtWithRefreshToken(string identityUserId)
        {
            IRefreshTokenService refreshTokenService = SecurityServiceFactory.GetService(typeof(IRefreshTokenService)) as IRefreshTokenService;
            IIdentityUserService identityUserService = SecurityServiceFactory.GetService(typeof(IIdentityUserService)) as IIdentityUserService;


            var applicationUser = await identityUserService.GetApplicationUser(identityUserId);

            var jwtToken = GenerateJwtToken(applicationUser);
            var refreshToken = refreshTokenService.GenerateAndSaveRefreshToken(identityUserId);
            return new JwtWithRefreshToken
            {
                AccessToken = jwtToken.AccessToken,
                Expires = jwtToken.Expires,
                Issued = jwtToken.Issued,
                RefreshToken = refreshToken,
            };

        }

        public Jwt GenerateJwtToken(ApplicationUser user)
        {
            var currentDateTime = DateTime.Now;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.ServerSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = currentDateTime.AddDays(Convert.ToDouble(jwtConfig.ExpiresIn));

            var token = new JwtSecurityToken(
                jwtConfig.Issuer,
                jwtConfig.Audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new Jwt
            {
                AccessToken = encodedJwt,
                Issued = currentDateTime,
                Expires = expires,
            };
        }

    }
}
