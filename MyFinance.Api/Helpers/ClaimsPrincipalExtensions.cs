using System;
using System.Linq;
using System.Security.Claims;

namespace MyFinance.Api.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserIdAsGuid(this ClaimsPrincipal user)
        {
            var userId = user.Claims.First(c => c.Type == "Id").Value;
            var userIdAsGuid = new Guid(userId);

            return userIdAsGuid;
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            var userName = user.Claims.First(c => c.Type == "UserName").Value;

            return userName;
        }
    }
}
