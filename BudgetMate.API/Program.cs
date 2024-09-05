using BudgetMate.API.Models;
using BudgetMate.Core.Contexts;
using BudgetMate.Core.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.Configure<MongoDBConfig>(builder.Configuration.GetSection("MongoDBConfig"));

builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBConfig>>().Value;
    return new ApplicationDBContext(settings.ConnectionURI, settings.DatabaseName);
});

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

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "SessionCookie";
        options.SlidingExpiration = true;
    });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
