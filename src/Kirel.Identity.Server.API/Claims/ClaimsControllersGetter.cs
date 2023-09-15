using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Kirel.Identity.Server.API.Claims;

/// <summary>
/// Simple static helper for get all controllers claims in current runtime
/// </summary>
public static class ClaimsControllersGetter
{
    /// <summary>
    /// Get all claim policies from controllers descriptions
    /// </summary>
    /// <param name="authorizationPolicyCollectionProvider"></param>
    /// <param name="actionDescriptorCollectionProvider"></param>
    /// <returns></returns>
    public static List<Claim> GetControllersClaimsFromPolicies(IAuthorizationPolicyProvider authorizationPolicyCollectionProvider,
        IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
    {
        var claims = new List<Claim>();
        var actionDescriptors = actionDescriptorCollectionProvider.ActionDescriptors.Items;
        foreach (var controllerActionDescriptor in actionDescriptors.OfType<ControllerActionDescriptor>())
        {
            foreach (var authorizeAttribute in controllerActionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>()
                         .Where(a => !string.IsNullOrEmpty(a.Policy)))
            {
                var policy = authorizationPolicyCollectionProvider
                    .GetPolicyAsync(authorizeAttribute.Policy!).Result;
                foreach (var requirement in policy!.Requirements.OfType<ClaimsAuthorizationRequirement>()
                             .Where(r => r.AllowedValues != null))
                {
                    claims.AddRange(requirement.AllowedValues!.Select(v => new Claim(requirement.ClaimType, v)));
                }
            }
        }
        return claims;
    }
}