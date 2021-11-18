using System;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScottBrady91.AspNetCore.Identity;
using SoarBeyond.Data;
using SoarBeyond.Data.Entities;
using SoarBeyond.Domain.AssemblyMarkers;
using SoarBeyond.Domain.AuthStateProviders;
using SoarBeyond.Domain.Providers;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Domain.Services;
using SoarBeyond.Domain.Services.Interfaces;
using SoarBeyond.Shared.ConfigurationOptions;
using SoarBeyond.Shared.Extensions;

namespace SoarBeyond.Domain
{
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
            services.AddTransient(p => p.GetRequiredService<IDbContextFactory<SoarBeyondDbContext>>().CreateDbContext());

            services.AddDefaultIdentity<SoarBeyondUserEntity>(options =>
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
            services.AddScoped<IPasswordHasher<SoarBeyondUserEntity>, BCryptPasswordHasher<SoarBeyondUserEntity>>();

            services.AddScoped<AuthenticationStateProvider, IdentityAuthStateProvider<SoarBeyondUserEntity>>();

            services.AddScoped<IIdentityService, IdentityService>();

            services.AddScoped<IJournalProvider, DbJournalProvider>();
            services.AddScoped<IJournalEntryProvider, DbJournalEntryProvider>();
            services.AddScoped<IThoughtProvider, DbThoughtProvider>();
            services.AddScoped<ISoarBeyondDbProvider, SoarBeyondDbProvider>();

            services.AddSingleton<IZenQuoteService, ZenQuoteService>();

            var assemblyMarker = typeof(IDomainAssemblyMarker);
            services.AddAutoMapper(assemblyMarker);
            services.AddMediatR(assemblyMarker);

            return services;
        }
    }
}