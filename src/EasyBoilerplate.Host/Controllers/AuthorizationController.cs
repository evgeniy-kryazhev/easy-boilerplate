using System.ComponentModel.DataAnnotations;
using EasyBoilerplate.Domain.Identity;
using EasyBoilerplate.Host.Extensions;
using EasyBoilerplate.Host.Validators;
using EasyBoilerplate.Host.ViewModels;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EasyBoilerplate.Host.Controllers;

[ApiController, Route("api/authorization")]
public class AuthorizationController(TimeProvider timeProvider,
    IOptionsMonitor<BearerTokenOptions> bearerTokenOptions,
    UserManager<ApplicationUser> userManager,
    IUserStore<ApplicationUser> userStore,
    SignInManager<ApplicationUser> signInManager) : ControllerBase
{
    private readonly EmailAddressAttribute _emailAddressAttribute = new();
    
    [HttpPost("login")]
    [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginAsync([FromForm] LoginPasswordViewModel viewModel)
    {
        var validator = new LoginPasswordValidator();
        var isValid = validator.Validate(viewModel);
        if (!isValid.IsValid && isValid.Errors.Any())
        {
            return Unauthorized(isValid.CreateValidationProblem());
        }
        var user = await userManager.FindByEmailAsync(viewModel.Email);
        if (user is null)
        { 
            return Unauthorized();
        }

        var isPassword = await userManager.CheckPasswordAsync(user, viewModel.Password);
        if (!isPassword)
        {
            return Unauthorized();
        }
        var principal = await signInManager.CreateUserPrincipalAsync(user);
        return SignIn(principal, authenticationScheme: IdentityConstants.BearerScheme);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromForm] RegisterViewModel viewModel)
    {
        if (!userManager.SupportsUserEmail)
        {
            throw new NotSupportedException($"{nameof(AuthorizationController)} requires a user store with email support.");
        }
        
        var emailStore = (IUserEmailStore<ApplicationUser>)userStore;
        if (string.IsNullOrEmpty(viewModel.Email) || !_emailAddressAttribute.IsValid(viewModel.Email))
        {
            var errors = IdentityResult
                .Failed(userManager.ErrorDescriber.InvalidEmail(viewModel.Email))
                .CreateValidationProblem(); 
            return BadRequest(errors);
        }
        
        var user = new ApplicationUser();
        await userStore.SetUserNameAsync(user, viewModel.Email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, viewModel.Email, CancellationToken.None);
        var result = await userManager.CreateAsync(user, viewModel.Password);

        if (result.Succeeded) return Ok();
        {
            var errors = result.CreateValidationProblem();
            return BadRequest(errors);
        }

    }
    
    [HttpPost("refresh"), Authorize]
    [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshRequest refreshRequest)
    {
        var refreshTokenProtector = bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
        var refreshTicket = refreshTokenProtector.Unprotect(refreshRequest.RefreshToken);
        if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
            timeProvider.GetUtcNow() >= expiresUtc ||
            await signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not { } user)
        {
            return Challenge();
        }

        var newPrincipal = await signInManager.CreateUserPrincipalAsync(user);
        return SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);
    }
}