using System.Text.Json;
using System.Text.Json.Serialization;
using AryDotNet.Authorization;
using AryDotNet.Azure.Extensions;
using AryDotNet.Module.Extensions;
using AryDotNet.Worker.Extension;
using Common.Config;
using Serilog;
using ShopifySharp;
using ShopifySharp.Extensions.DependencyInjection;
using TagClient.Client;
using TagClient.Extensions;
using TagClient.Models.Configurations;
using VideoTag;
using VideoTag.Auth;
using VideoTag.Middlewares;
using VideoTag.Mock;
using VideoTag.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationInsightsTelemetry();
builder.Host.UseSerilog((_, configuration) => configuration.ReadFrom.Configuration(builder.Configuration));
builder.Services.AddHttpContextAccessor();
builder.Services.InitModules(builder.Configuration, Module.RegisteredModules);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Module.RegisteredModules));
builder.Services.ConfigureBackgroundWorker(capacity: 50);
builder.Services.AddScoped<IAuthContext, DefaultAuthContext>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase,
            false));
    });
builder.Services.AddSwaggerGen();

var allowedHosts = builder.Configuration["Cors"] ?? "http://localhost:3000";
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp",
        corsPolicyBuilder =>
            corsPolicyBuilder.WithOrigins(allowedHosts)
                .AllowAnyHeader()
                .AllowCredentials()
                .AllowAnyMethod()
    );
});

if (builder.Configuration.GetSection("Shopify").Exists())
{
    builder.Services.Configure<ShopifyConfig>(builder.Configuration.GetSection("Shopify"));
}

builder.Services.AddShopifySharp<LeakyBucketExecutionPolicy>();
builder.Services.AddShopifySharpServiceFactories();
builder.Services.AddShopifySharpUtilities();
builder.Services.AddBlobStorage(builder.Configuration.GetSection("Storage"));
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddAuthentication(ShopifyAuthenticationHandler.SchemeName)
    .AddScheme<ShopifyAuthenticationOptions, ShopifyAuthenticationHandler>(ShopifyAuthenticationHandler.SchemeName,
        options => { });

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<ITagClient, TagClientMock>();
}
else
{
    var tagConfig = builder.Configuration.GetSection("TagBackend").Get<TagApiSettings>();
    if (tagConfig != null)
    {
        builder.Services.AddTagService(tagConfig);
    }
}

var app = builder.Build();
app.UseCors("AllowWebApp");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();