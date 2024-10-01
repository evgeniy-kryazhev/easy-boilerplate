using Microsoft.AspNetCore.Identity;

namespace EasyBoilerplate.Domain.Identity;

public class ApplicationPasswordValidator : IPasswordValidator<ApplicationUser>
{
    public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string? password)
    {
        return Task.FromResult(IdentityResult.Success);
    }
}