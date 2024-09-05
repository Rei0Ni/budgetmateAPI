using BudgetMate.API.Models;
using BudgetMate.Application.Configuration;
using BudgetMate.Core.Contexts;
using BudgetMate.Core.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Default", LogEventLevel.Fatal)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
    .MinimumLevel.Override("System", LogEventLevel.Fatal)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"logs\\Info\\Info-{DateTime.Now:dd-MM-yyyy}.log"), LogEventLevel.Information, "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"logs\\Debug\\Debug-{DateTime.Now:dd-MM-yyyy}.log"), LogEventLevel.Debug, "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"logs\\Warning\\Warning-{DateTime.Now:dd-MM-yyyy}.log"), LogEventLevel.Warning, "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"logs\\Error\\Error-{DateTime.Now:dd-MM-yyyy}.log"), LogEventLevel.Error, "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"logs\\Fatal\\Fatal-{DateTime.Now:dd-MM-yyyy}.log"), LogEventLevel.Fatal, "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"))
    .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"logs\\Verbose\\log-{DateTime.Now:dd-MM-yyyy}.log"))
    .CreateLogger();

builder.Logging.ClearProviders();

builder.Services.AddLogging();

Log.Logger = logger;

//builder.Logging.AddSerilog(logger);
builder.Host.UseSerilog(logger);

var configuration = builder.Configuration;

builder.Services.Configure<MongoDBConfig>(builder.Configuration.GetSection("MongoDBConfig"));

builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBConfig>>().Value;
    return new ApplicationDBContext(settings.ConnectionURI, settings.DatabaseName);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.Name = "SessionCookie";
});

// builder.Services.ConfigureApplicationCookie(options =>
// {
//     options.Cookie.Name = "SessionCookie";
// });

// AddIdentity :-  Registers the services  
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(config =>
{
    // User defined password policy settings.  
    config.Password.RequiredLength = 4;
    config.Password.RequireDigit = false;
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequireUppercase = false;

    config.User.RequireUniqueEmail = false;

    config.Lockout.AllowedForNewUsers = false;
    config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    config.Lockout.MaxFailedAccessAttempts = 5;
})
    .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
    (
        configuration["MongoDBConfig:ConnectionURI"], configuration["MongoDBConfig:DatabaseName"]
    )
    .AddDefaultTokenProviders();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddApplicationCore();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var roleManager =
        scope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();
    var userManager =
        scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

    var roles = Enum.GetNames(typeof(Role)).ToList();

    foreach (var role in roles)
    {
        if (!await roleManager!.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new ApplicationRole() { Name = role });
        }
    }

    string email = "abdelrahman.hamdy.hashim@gmail.com";
    string username = "Administrator";
    string password = "P@ssw0rd";

    if (await userManager!.FindByEmailAsync(email) == null)
    {
        ApplicationUser default_administrator = new()
        {
            UserName = username,
            Email = email,
            EmailConfirmed = true,
        };
        await userManager.CreateAsync(default_administrator, password);
        await userManager.AddToRoleAsync(default_administrator,
                    Enum.GetName(typeof(Role), Role.Administrator)!);
    }
}

app.Run();
