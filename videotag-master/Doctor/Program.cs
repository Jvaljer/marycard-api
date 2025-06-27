using Doctor;
using FluentArgs;
using Microsoft.Extensions.Configuration;
using VideoTag;

await FluentArgsBuilder.New()
    .Parameter<string>("-f", "--file")
    .WithDescription("App settings json configuration")
    .IsOptional()
    .Flag("-i", "--install")
    .WithDescription("Run modules installation")
    .Call(install => file =>
    {
        if (install)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), file);
            return InstallModules(path);
        }

        return Task.CompletedTask;
    }).ParseAsync(args);

return;

async Task InstallModules(string? configPath)
{
    var builder = new ConfigurationBuilder();
    if (configPath != null)
    {
        builder.AddJsonFile(configPath);
    }
    var configuration = builder.AddEnvironmentVariables()
        .Build();

    await Installer.Install(configuration, Module.RegisteredModules);
}