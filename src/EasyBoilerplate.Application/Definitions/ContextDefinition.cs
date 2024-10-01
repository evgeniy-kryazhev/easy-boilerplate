using Calabonga.AspNetCore.AppDefinitions;
using EasyBoilerplate.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyBoilerplate.Application.Definitions;

public class ContextDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("Default");
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
    }
}