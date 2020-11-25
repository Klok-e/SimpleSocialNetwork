using System.Security.Claims;

namespace Business.Common
{
    public class TypedClaimsPrincipal
    {
        public TypedClaimsPrincipal(ClaimsPrincipal claims)
        {
            Name = claims.Identity?.Name;
            Role = claims.HasClaim(ClaimTypes.Role, Roles.Admin) ? Roles.Admin : Roles.User;
        }

        public string? Name { get; }

        public string? Role { get; }
    }
}