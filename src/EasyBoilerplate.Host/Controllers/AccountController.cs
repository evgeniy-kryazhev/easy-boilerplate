using AutoMapper;
using EasyBoilerplate.Application.Services.Identity;
using EasyBoilerplate.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EasyBoilerplate.Host.Controllers;

[ApiController, Route("api/account")]
public class AccountController(
    ICurrentUserService currentUserService,
    ApplicationUserManager userManager,
    IMapper mapper) : ControllerBase
{
    [HttpGet, Authorize]
    public async Task<ApplicationUserDto?> GetAsync()
    {
        var user = await userManager.FindByIdAsync(currentUserService.GetId().ToString());
        return mapper.Map<ApplicationUser?, ApplicationUserDto?>(user);
    }
}