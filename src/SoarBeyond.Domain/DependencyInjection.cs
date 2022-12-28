using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScottBrady91.AspNetCore.Identity;
using SoarBeyond.Data;
using SoarBeyond.Data.Entities;
using SoarBeyond.Data.Seed;
using SoarBeyond.Domain.AssemblyMarkers;
using SoarBeyond.Domain.AuthStateProviders;
using SoarBeyond.Domain.Providers;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Domain.Services;
using SoarBeyond.Domain.Services.Interfaces;
using SoarBeyond.Shared.ConfigurationOptions;
using SoarBeyond.Shared.Extensions;

namespace SoarBeyond.Domain;

public static class DependencyInjection
{
    private const string InMemoryDbName = "SoarBeyondInMemoryDb";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var soarBeyondSection = configuration.GetSoarBeyondConfigurationOptionsSection();
        services.Configure<SoarBeyondConfigurationOptions>(soarBeyondSection);

        var tempOptions = soarBeyondSection.GetSoarBeyondConfigurationOptions(soarBeyondSection);

        var useInMemDb = tempOptions.Persistence.UseInMemDb;
        var connectionString = tempOptions.GetConnectionString();

        services.AddDbContextFactory<SoarBeyondDbContext>(options =>
        {
#if DEBUG
            if (useInMemDb)
            {
                options.UseInMemoryDatabase(InMemoryDbName)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
            else
            {
                var detailedErrorConnString = connectionString + "Include Error Detail=true;";
                options.UseNpgsql(detailedErrorConnString)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
#else
                options.UseNpgsql(connectionString);
#endif
        });

        services.AddDefaultIdentity<UserEntity>(options =>
            {
                /* Todo: Add Email conformation requirement for accounts */
                options.User.RequireUniqueEmail = true;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(7);
            })
            .AddEntityFrameworkStores<SoarBeyondDbContext>();

        /* Todo: Add mechanism for upgrading/changing hashing algorithm (stronger/better) */
        services.Configure<BCryptPasswordHasherOptions>(options => options.WorkFactor = 13);
        services.AddScoped<IPasswordHasher<UserEntity>, BCryptPasswordHasher<UserEntity>>();

        services.AddScoped<AuthenticationStateProvider, IdentityAuthStateProvider<UserEntity>>();

        services.AddScoped<ICategoryProvider, DbCategoryProvider>();
        services.AddScoped<IJournalProvider, DbJournalProvider>();
        services.AddScoped<IMomentProvider, DbMomentProvider>();
        services.AddScoped<INoteProvider, DbNoteProvider>();

        services.AddSingleton<IZenQuoteService, ZenQuoteService>();

        var assemblyMarker = typeof(IDomainAssemblyMarker);
        services.AddAutoMapper(assemblyMarker);
        services.AddMediatR(assemblyMarker);

        return services;
    }

    public static async Task MigrateDatabaseAsync(this IHost host)
    {
        using var serviceScope = host.Services.CreateScope();
        await using var dbContext = serviceScope.ServiceProvider.GetRequiredService<SoarBeyondDbContext>();

        var isInMemDb = dbContext.Database.IsInMemory();
        if (isInMemDb)
        {
            await dbContext.Database.EnsureCreatedAsync();
        }
        else
        {
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any()) await dbContext.Database.MigrateAsync();
        }

#if DEBUG
        await dbContext.SeedDatabase();
#endif
    }
}