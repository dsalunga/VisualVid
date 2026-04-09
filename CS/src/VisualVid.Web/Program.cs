using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VisualVid.Web.Data;
using VisualVid.Web.Middleware;
using VisualVid.Web.Models;
using VisualVid.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Database — supports both SQL Server and PostgreSQL via "DatabaseProvider" setting
var dbProvider = builder.Configuration.GetValue<string>("DatabaseProvider") ?? "SqlServer";
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (dbProvider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase))
        options.UseNpgsql(connectionString);
    else
        options.UseSqlServer(connectionString);
});

// Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Cookie configuration matching legacy forms auth behavior
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
        ? CookieSecurePolicy.SameAsRequest
        : CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// Application services
builder.Services.AddScoped<VideoService>();
builder.Services.AddScoped<MemberService>();

// Email
var emailSettings = builder.Configuration.GetSection("Email").Get<EmailSettings>() ?? new EmailSettings();
builder.Services.AddSingleton(emailSettings);
builder.Services.AddScoped<EmailService>();

// MVC
builder.Services.AddControllersWithViews();

// Increase multipart form body limit for large video uploads (512 MB)
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 536_870_912; // 512 MB
});

// Anti-forgery
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
        ? CookieSecurePolicy.SameAsRequest
        : CookieSecurePolicy.Always;
});

// Health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Observability: request metrics (before other middleware so it captures everything)
app.UseMiddleware<RequestMetricsMiddleware>();

// Security headers
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    context.Response.Headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");

// Observability metrics endpoint (admin only in production)
app.MapGet("/metrics", (HttpContext context) =>
{
    var snapshot = RequestMetricsMiddleware.GetSnapshot();
    return Results.Json(snapshot);
}).RequireAuthorization(policy => policy.RequireRole("Admin"));

// Route mappings — compatibility redirects for old URLs
app.MapControllerRoute(
    name: "watch",
    pattern: "Watch.aspx",
    defaults: new { controller = "Video", action = "Watch" });

app.MapControllerRoute(
    name: "profile",
    pattern: "Profile.aspx",
    defaults: new { controller = "Profile", action = "Index" });

app.MapControllerRoute(
    name: "results",
    pattern: "Results.aspx",
    defaults: new { controller = "Search", action = "Index" });

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
