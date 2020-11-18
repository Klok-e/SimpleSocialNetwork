using System.Security.Claims;

namespace Business.Common
{
    public class TypedClaimsPrincipal
    {
        public string? Name { get; }

        public TypedClaimsPrincipal(ClaimsPrincipal claims)
        {
            Name = claims.Identity?.Name;
        }
    }
}