using Calabonga.AspNetCore.AppDefinitions;
using EasyBoilerplate.Application.Services.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EasyBoilerplate.Application.Definitions;

public class ServicesDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddAutoMapper(typeof(ApplicationProfile));
        builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
    }
}