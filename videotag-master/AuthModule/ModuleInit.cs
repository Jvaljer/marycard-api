using AryDotNet.Module;
using AuthModule.Config;
using AuthModule.Domain;
using AuthModule.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthModule;

internal sealed class ModuleInit : IModuleInit
{
    public void Init(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var config = configuration.GetSection("AuthModule").Get<AuthConfig>();
        if (config is null)
        {
            return;
        }

        serviceCollection.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        });

        serviceCollection.AddDbContext<AuthDbContext>();
        serviceCollection.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddTokenProvider<EmailTokenProvider<User>>(TokenOptions.DefaultProvider);

        serviceCollection.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedAccount = config.RequireConfirmedAccount;
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.MaxFailedAccessAttempts = config.MaxFailedAccessAttempts;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(config.DefaultLockoutTimeSpan);
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredUniqueChars = 0;
        });

        serviceCollection.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.Name = "marycard";
            options.SlidingExpiration = true;
            options.Cookie.SameSite = config.SameSite;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.ExpireTimeSpan = TimeSpan.FromHours(config.CookieExpireTimeSpan);
            options.Events.OnRedirectToLogin = ctx =>
            {
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                ctx.Response.WriteAsJsonAsync(new
                {
                    Success = false,
                    Error = "User authentication required"
                });
                return Task.CompletedTask;
            };
            options.Events.OnRedirectToAccessDenied = ctx =>
            {
                ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                ctx.Response.WriteAsJsonAsync(new
                {
                    Success = false,
                    Error = "Resource requires privilege"
                });
                return Task.CompletedTask;
            };
        });

    }
}