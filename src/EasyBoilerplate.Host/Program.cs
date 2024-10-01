using Calabonga.AspNetCore.AppDefinitions;
using EasyBoilerplate.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    builder.AddDefinitions(typeof(ApplicationExtensions));
    builder.Services.AddHttpClient();
    
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer();
    
    builder.Services.AddAuthorization();
    builder.Services.AddSerilog();
    builder.Services.AddCors();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    
    builder.Services.AddSwaggerGen(opt =>
    {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ExCraft", Version = "v1" });
        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },[]
            }
        });
    });
    
    var app = builder.Build();
    
    app.UseSwagger();
    app.UseSwaggerUI(settings =>
    {
        settings.DocExpansion(DocExpansion.None);
    });
    
    app.UseDefinitions();
    app.UseSerilogRequestLogging();
    
    app.UseCors(policyBuilder => policyBuilder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
    );

    app.MapGet("/", () => Results.LocalRedirect("/swagger"))
        .ExcludeFromDescription();

    app.MapControllers()
        .WithDescription("api");

    app.Run();
}
catch (Exception ex)
{
    Log.Error(ex, "Unhandled exception");
}
finally
{
    await Log.CloseAndFlushAsync();
}