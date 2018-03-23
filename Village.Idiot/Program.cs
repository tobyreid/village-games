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
                .UseApplicationInsights()
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
