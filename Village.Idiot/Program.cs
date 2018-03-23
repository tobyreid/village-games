using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Village.Idiot.Data;

namespace Village.Idiot
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

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
        {
            public ApplicationDbContext CreateDbContext(string[] args)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                builder.UseSqlServer(connectionString);
                return new ApplicationDbContext(builder.Options);
            }
        }
    }
}
