using System;
using System.Linq;
using System.Security.Claims;

namespace MyFinance.Api.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? GetUserIdAsGuid(this ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim == null)
                return null;

            var userIdAsGuid = new Guid(userIdClaim.Value);

            return userIdAsGuid;
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            var userNameClaim = user.Claims.FirstOrDefault(c => c.Type == "UserName");

            if (userNameClaim == null)
                return null;

            return userNameClaim.Value;
        }
    }
}
