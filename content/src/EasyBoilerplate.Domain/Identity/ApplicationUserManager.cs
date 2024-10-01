using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EasyBoilerplate.Domain.Identity;

public class ApplicationUserManager(
    IUserStore<ApplicationUser> store,
    IOptions<IdentityOptions> optionsAccessor,
    IPasswordHasher<ApplicationUser> passwordHasher,
    IEnumerable<IUserValidator<ApplicationUser>> userValidators,
    IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
    ILookupNormalizer keyNormalizer,
    IdentityErrorDescriber errors,
    IServiceProvider services,
    ILogger<UserManager<ApplicationUser>> logger)
    : UserManager<ApplicationUser>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators,
        keyNormalizer, errors, services, logger)
{
    public override Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
    {
        ThrowIfDisposed();
        return GenerateUserTokenAsync(user, "otp", ResetPasswordTokenPurpose);
    }
    
    public override async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        
        if (!await VerifyUserTokenAsync(user, "otp", ResetPasswordTokenPurpose, token).ConfigureAwait(false))
        {
            return IdentityResult.Failed(ErrorDescriber.InvalidToken());
        }
        var result = await UpdatePasswordHash(user, newPassword, validatePassword: true).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            return result;
        }
        return await UpdateUserAsync(user).ConfigureAwait(false);
    }
}