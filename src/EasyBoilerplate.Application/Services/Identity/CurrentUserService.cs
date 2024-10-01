using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace EasyBoilerplate.Application.Services.Identity;

public class CurrentUserService(IHttpContextAccessor contextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal GetPrincipal()
    {
        var principal = contextAccessor.HttpContext?.User;
        if (principal is { Identity.IsAuthenticated: false })
        {
            throw new UnauthorizedAccessException();
        }

        return principal!;
    }
    
    public Guid GetId()
    {
        var principal = GetPrincipal();
        var idClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        return Guid.Parse(idClaim!.Value);
    }
}