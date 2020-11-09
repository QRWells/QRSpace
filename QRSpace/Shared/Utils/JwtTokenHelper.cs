using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace QRSpace.Shared.Utils
{
    public static class JwtTokenHelper
    {
        public static string IssueJwtToken(string userName, List<string> roles, IConfigurationSection configuration)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(JwtRegisteredClaimNames.Jti, userName),
                new Claim(JwtRegisteredClaimNames.Iss, configuration["Issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud, configuration["Audience"]),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddSeconds(1000)).ToUnixTimeSeconds()}")
            };
            if (roles.Count == 1)
            {
                claims.Add(new Claim(ClaimTypes.Role, roles.FirstOrDefault()));
            }
            else
            {
                claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecurityKey"]));
            var credos = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: configuration["Issuer"],
                claims: claims,
                signingCredentials: credos
            );
            var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodeJwt;
        }
    }
}