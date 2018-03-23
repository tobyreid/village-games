using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Village.Games
{
    public class Program
    {
        public static void Main(string[] args)
        {

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((context, config) =>
                {
                    if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "azurekeyvault.json")))
                    {
                        config.SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("azurekeyvault.json", false, true)
                            .AddEnvironmentVariables();

                        var builtConfig = config.Build();

                        config.AddAzureKeyVault(
                            $"https://{builtConfig["azureKeyVault:vault"]}.vault.azure.net/",
                            builtConfig["azureKeyVault:clientId"],
                            builtConfig["azureKeyVault:clientSecret"]
                        );
                    }
                })
                .Build();
    }
}
