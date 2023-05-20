using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Movies.Api.Common.Tokens;
using Owin;
using System.Text;

[assembly: OwinStartup(typeof(Movies.Api.StartUp))]

namespace Movies.Api
{
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = TokenHelper.JwtIssuer,
                        ValidAudience = TokenHelper.JwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenHelper.JwtKey))
                    }
                });
        }
    }
}
