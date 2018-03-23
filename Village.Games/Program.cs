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
                    config.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("Azurekeyvault.json", true, true)
                        .AddEnvironmentVariables();

                    var builtConfig = config.Build();
                    var clientId = builtConfig["AzureKeyVault:clientId"];
                    var clientSecret = builtConfig["AzureKeyVault:clientSecret"];
                    var vault = builtConfig["AzureKeyVault:vault"];
                    if (!string.IsNullOrWhiteSpace(vault))
                    {
                        config.AddAzureKeyVault(
                            $"https://{vault}.vault.azure.net/",
                            clientId,
                            clientSecret
                        );
                    }
                    
                })
                .Build();
    }
}
