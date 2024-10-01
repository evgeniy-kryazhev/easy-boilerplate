using Calabonga.AspNetCore.AppDefinitions;
using EasyBoilerplate.Domain.Identity;
using EasyBoilerplate.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EasyBoilerplate.Application.Definitions;

public class IdentityDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IPasswordValidator<ApplicationUser>, ApplicationPasswordValidator>();
        builder.Services.AddIdentityCore<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddUserManager<ApplicationUserManager>()
            .AddRoleManager<RoleManager<ApplicationRole>>()
            .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
            .AddDefaultTokenProviders();
        
        builder.Services
            .AddIdentityApiEndpoints<ApplicationUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
    }
}