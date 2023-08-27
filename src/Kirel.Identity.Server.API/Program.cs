using Kirel.Identity.Middlewares;
using Kirel.Identity.Server.API.Controllers;
using Kirel.Identity.Server.API.Extensions;
using Kirel.Identity.Server.API.Filters;
using Kirel.Identity.Server.API.Handlers;
using Kirel.Identity.Server.Core.Extensions;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.Infrastructure.Contexts;
using Kirel.Identity.Server.Infrastructure.Shared.Extensions;
using Kirel.Identity.Server.Infrastructure.Shared.Models;
using Kirel.Identity.Server.Jwt.Shared;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

var jwtConfig = builder.Configuration.GetSection("jwt").Get<JwtAuthenticationConfig>();
var dbConfig = builder.Configuration.GetSection("dbConfig").Get<DbConfig>();
var maintenanceConfig = builder.Configuration.GetSection("maintenance").Get<MaintenanceConfig>();
var registrationDisabled = builder.Configuration.GetValue<bool>("RegistrationDisabled");
var apiKeys = builder.Configuration.GetSection("APIKeys").Get<ApiKeysList>();

//To disable controller add him here like disabledControllers.Add(typeof(RegistrationController));
var disabledControllers = new DisabledControllerTypes();
if (registrationDisabled) disabledControllers.Add(typeof(RegistrationController));

// Add Identity framework db context based on Kirel Identity templates
// with ability to change db driver (mssql, mysql, postgresql)
builder.Services.AddDbContextByConfig<IdentityServerDbContext, MssqlIdentityServerDbContext,
    PostgresqlIdentityServerDbContext, MysqlIdentityServerDbContext, SqliteIdentityServerDbContext>(dbConfig);
builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<IdentityServerDbContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;

    // User settings.
    options.User.RequireUniqueEmail = false;
});

// Add identity mappers
builder.Services.AddMappers();
// Add identity DTOs fluent validators
builder.Services.AddValidators();
// Add Identity services and jwt token issuing service, that works with DTOs and with fluent validation framework
builder.Services.AddServices(jwtConfig);
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new EnabledControllerActionFilter(disabledControllers));
});


// Add ASP.NET authentication configuration
builder.Services.AddAuthenticationConfiguration(jwtConfig, apiKeys);

// Add custom swagger configuration
builder.Services.AddSwagger(disabledControllers);

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCorsPolicy", corsBuilder =>
    {
        corsBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders();
    });
});

var app = builder.Build();

// Apply migrations to db
await app.MigrateIdentityDbAsync();
// Create admin user and role, do maintenance admin password reset if needed
await app.MaintenanceAsync<Guid, User, Role, UserRole>(maintenanceConfig);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DevCorsPolicy");
//app.UseHttpsRedirection();

// Save order, UseAuthentication() - First.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Middleware that handle all exceptions throws and set response code depends on exception type
app.UseMiddleware<KirelErrorHandlerMiddleware>();

app.Run();