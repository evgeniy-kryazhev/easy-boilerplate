using Calabonga.AspNetCore.AppDefinitions;
using ExCraft.Host.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;

namespace EasyBoilerplate.Application.Definitions;

public class ConfigDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        var smtpConfigSection = builder.Configuration.GetSection("smtp");
        builder.Services.Configure<S3Config>(builder.Configuration.GetSection("s3"));
        builder.Services.Configure<SmsConfig>(builder.Configuration.GetSection("sms"));
        builder.Services.Configure<SmtpConfig>(smtpConfigSection);

        builder.Services.Configure<FormOptions>(o =>
        {
            o.ValueLengthLimit = int.MaxValue;
            o.MultipartBodyLengthLimit = int.MaxValue;
            o.MemoryBufferThreshold = int.MaxValue;
        });
    }
}