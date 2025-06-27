using System.Reflection;
using AryDotNet.Module;
using Microsoft.Extensions.Configuration;

namespace Doctor;

internal static class Installer
{
    public static async Task Install(IConfiguration configuration, IEnumerable<Assembly> modules)
    {
        var type = typeof(IModuleInstaller);
        var types = modules.SelectMany(a => a.GetTypes())
            .Where(t => type.IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });
        foreach (var t in types)
        {
            var instance = (IModuleInstaller?)Activator.CreateInstance(t);
            if (instance == null) continue;

            Console.Write($"Installing {t.FullName}...");
            await instance.Install(configuration);
            Console.WriteLine("\tsuccess");
        }
    }
}