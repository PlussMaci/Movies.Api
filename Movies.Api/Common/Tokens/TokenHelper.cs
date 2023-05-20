using Microsoft.IdentityModel.Tokens;
using Movies.Api.Common.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Movies.Api.Common.Tokens
{
    public static class TokenHelper
    {
        public static string JwtKey => ConfigurationManager.AppSettings["JwtKey"];
        public static string JwtIssuer => ConfigurationManager.AppSettings["JwtIssuer"];
        public static string JwtAudience => ConfigurationManager.AppSettings["JwtAudience"];

        public static string GetToken(User user, List<string> roles, out DateTime expires)
        {
            expires = DateTime.Now.AddDays(1);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var permClaims = new List<Claim>();

            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Name, user.Name));
            permClaims.Add(new Claim("userID", user.ID.ToString()));

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    permClaims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
                }
            }

            var token = new JwtSecurityToken(JwtIssuer,
                            JwtAudience,
                            permClaims,
                            expires: expires,
                            signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}