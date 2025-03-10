﻿namespace Linn.Stores2.Service.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;

    public static class ClaimsPrincipalExtensions
    {
        public static IEnumerable<string> GetPrivileges(this HttpContext context)
        {
            return context.User?.Claims?.Where(b => b.Type == "privilege").Select(a => a.Value);
        }

        public static string GetEmployeeUrl(this ClaimsPrincipal principal)
        {
            return principal?.Claims
                .FirstOrDefault(claim => claim.Type.Equals("employee", StringComparison.InvariantCultureIgnoreCase))
                ?.Value;
        }

        public static int? GetEmployeeNumber(this ClaimsPrincipal principal)
        {
            var url = principal.GetEmployeeUrl();
            return string.IsNullOrEmpty(url) ? null : int.Parse(url.Split("/").Last());
        }
            
        public static bool HasClaim(this ClaimsPrincipal principal, string type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return principal.Identities.Any(identity => identity.HasClaim(type));
        }
    }
}
