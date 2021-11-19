using Microsoft.EntityFrameworkCore;
using SoarBeyond.Data;
using SoarBeyond.Data.Seed;

namespace SoarBeyond.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using var serviceScope = host.Services?.CreateScope();
        if (serviceScope is not null)
        {
            await using var dbContext = serviceScope.ServiceProvider
                .GetRequiredService<SoarBeyondDbContext>();

            var isInMemDb = dbContext.Database.IsInMemory();
            if (isInMemDb)
            {
                await dbContext.Database.EnsureCreatedAsync();
            }
            else
            {
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    await dbContext.Database.MigrateAsync();
                }
            }

#if DEBUG
            await dbContext.SeedDatabase();
#endif
        }

        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, configuration) =>
            {
                /* Temporary fix for Windows 'dotnet watch' not loading user secrets configuration */
                configuration.AddUserSecrets<Startup>(optional: true);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}