using Library.Models.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using WebClient.Models;
using WebClient.Services;
using WebClient.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddSessionStateTempDataProvider();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Account/Login");
        options.Cookie.Name = "AuthCookie";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromDays(builder.Configuration.GetValue<int>("JwtSettings:ExpiresInDays"));
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddHttpClient("AnnouncementApi",
    client => { client.BaseAddress = new Uri("https://localhost:7014/"); });
builder.Services.AddHttpClient("AuthApi", client => { client.BaseAddress = new Uri("https://localhost:7017/"); });

builder.Services.Configure<GoogleAuthOptions>(
    builder.Configuration.GetSection("OAuth"));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();

Log.Logger = new LoggerConfiguration()
    .Enrich.WithProperty("LogTime", DateTime.UtcNow)
    .WriteTo.Console(outputTemplate: "[{Level:u3}]: {Message:lj} - {LogTime:yyyy-MM-dd HH:mm:ss}{NewLine}{NewLine}")
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always,
    MinimumSameSitePolicy = SameSiteMode.Strict
});

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Announcement}/{action=Index}")
    .WithStaticAssets();

await app.RunAsync();