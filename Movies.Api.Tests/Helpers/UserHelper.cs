using Movies.Api.Common.Entities;
using Movies.Api.Common.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Movies.Api.Tests.Helpers
{
    internal static class UserHelper
    {
        internal static void SetUser(out Guid userID, ApiController controller, bool isAdmin)
        {
            userID = Guid.NewGuid();
            var roles = new List<string>() { "user" };
            if (isAdmin)
            {
                roles.Add("admin");
            }

            var token = TokenHelper.GetToken(new User()
            {
                ID = userID,
                Name = "test",
                Password = "Test"
            },
                    roles,
                    out _);

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var user = new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims, "JWT"));

            controller.ControllerContext.RequestContext.Principal = user;
        }
    }
}
