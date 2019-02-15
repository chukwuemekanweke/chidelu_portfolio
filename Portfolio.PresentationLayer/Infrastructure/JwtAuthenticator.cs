using BoilerPlate.ModelLayer.Identity;
using BoilerPlate.Security.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebBackendBoilerPlate.Infrastructure
{
    public class JwtAuthenticator
    {
        readonly JwtConfiguration jwtConfig;
        public JwtAuthenticator(JwtConfiguration _jwtConfig)
        {
            jwtConfig = _jwtConfig;
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

    public class Jwt
    {
        public string AccessToken { get; set; }
        public DateTime Issued { get; set; }
        public DateTime Expires { get; set; }
    }
}
