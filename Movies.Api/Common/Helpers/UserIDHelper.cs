using System;
using System.Linq;
using System.Security.Principal;

namespace Movies.Api.Common.Helpers
{
    public static class UserIDHelper
    {
        public static Guid GetUserID(IPrincipal currentUser)
        {
            var token = (currentUser?.Identity as System.Security.Claims.ClaimsIdentity)?.Claims?.FirstOrDefault(x => x.Type == "userID")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                return Guid.Empty;
            }

            return Guid.Parse(token);
        }
    }
}