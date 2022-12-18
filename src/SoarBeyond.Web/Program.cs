using Blazored.Toast;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using SoarBeyond.Components;
using SoarBeyond.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddServerSideBlazor();

#if DEBUG
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
#endif

builder.Services.AddHttpClient();
builder.Services.AddBlazoredToast();
builder.Services.AddScoped<SoarBeyondJsInterop>();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
        "application/octet-stream",
    });
});

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseResponseCompression();

if (app.Environment.IsDevelopment())
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

if (app.Environment.IsProduction())
{
    // Reverse proxy
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                           ForwardedHeaders.XForwardedProto
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub(options =>
    options.WebSockets.CloseTimeout = TimeSpan.FromMinutes(30));

app.MapControllers();
app.MapFallbackToPage("/_Host");

await app.MigrateDatabaseAsync();
await app.RunAsync();