using AryDotNet.Module;
using AuthModule.Domain;
using AuthModule.Infrastructure;
using Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AuthModule;

internal sealed class ModuleInstaller : IModuleInstaller
{
    private async Task SeedRoles(AuthDbContext ctx)
    {
        var roleStore = new RoleStore<IdentityRole<Guid>, AuthDbContext, Guid>(ctx);

        foreach (var role in Role.Roles)
        {
            bool roleExists = await roleStore.Roles.AnyAsync(r => r.Name == role);
            if (!roleExists)
            {
                await roleStore.CreateAsync(new IdentityRole<Guid>
                {
                    Name = role,
                    NormalizedName = role.Normalize(),
                    ConcurrencyStamp = Guid.NewGuid().ToString("N"),
                });
            }
        }
    }

    private async Task SeedAdmin(AuthDbContext ctx)
    {
        var userManager = new UserStore<User, IdentityRole<Guid>, AuthDbContext, Guid>(ctx);

        var adminExists = await userManager.Users.AnyAsync(u => u.UserName == "admin");

        if (!adminExists)
        {
            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                Email = "admin@ary.eu",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                EmailConfirmed = true,
                NormalizedEmail = "ADMIN@ARY.EU",
                SecurityStamp = Guid.NewGuid().ToString("N"),
                LockoutEnabled = true,
            };

            user.PasswordHash = hasher.HashPassword(user, "!AryAdmin1234!");

            await userManager.CreateAsync(user);
        }
    }

    public async Task Install(IConfiguration configuration)
    {
        var db = new AuthDbContext(configuration);
        await db.Database.MigrateAsync();
        await SeedRoles(db);
        await SeedAdmin(db);
    }
}