using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;

namespace MyFinance.Api.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserIdAsGuid(this ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(k => k.Type.Contains("nameidentifier"));

            if (userIdClaim == null)
                return Guid.NewGuid();

            var userIdAsGuid = new Guid(userIdClaim.Value);

            return userIdAsGuid;
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            var userNameClaim = user.Claims.FirstOrDefault(c => c.Type == "cognito:username");

            if (userNameClaim == null)
                return null;

            return userNameClaim.Value;
        }
    }
}
