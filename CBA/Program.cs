using CBA.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBA
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("app");
                try
                {
                    var userManager = services.GetRequiredService<UserManager<CBAUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<CBARole>>();
                    await SeedData.SeedRolesAsync(userManager, roleManager);
                    logger.LogInformation("SeedData for Default roles successful");
                    await SeedData.SeedSuperUserAsync(userManager, roleManager);
                    logger.LogInformation("SeedData for Default users successful");
                    logger.LogInformation("Application Starting");
                }
                catch (Exception e)
                {
                    logger.LogWarning(e, "An error occurred seeding the database");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
