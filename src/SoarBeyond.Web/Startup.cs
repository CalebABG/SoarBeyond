using Blazored.Toast;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using SoarBeyond.Components;
using SoarBeyond.Domain;

namespace SoarBeyond.Web;

public class Startup
{
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddServerSideBlazor();
        services.AddRazorPages();
        services.AddControllers();

#if DEBUG
        services.AddDatabaseDeveloperPageExceptionFilter();
#endif

        services.AddHttpClient();
        services.AddBlazoredToast();

        services.AddScoped<SoarBeyondJsInterop>();

        services.AddResponseCompression(opts =>
        {
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
            {
                "application/octet-stream",
            });
        });

        services.AddInfrastructure(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseResponseCompression();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        if (env.IsProduction()) // Reverse proxy
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                   ForwardedHeaders.XForwardedProto
            });
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapBlazorHub(options =>
            {
                /* Potential solution for Blazor server frequently disconnecting
                 * and reconnecting after ~30-45 sec
                 * https://stackoverflow.com/questions/60057826/blazor-server-side-app-on-iis-frequently-disconnects-websocket-connection
                 */
                options.WebSockets.CloseTimeout = TimeSpan.FromMinutes(30);
            });
            endpoints.MapControllers();
            endpoints.MapFallbackToPage("/_Host");
        });
    }
}