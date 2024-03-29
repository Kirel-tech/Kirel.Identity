using Kirel.Identity.Controllers.Extensions;
using Kirel.Identity.Middlewares;
using Kirel.Identity.Server.API.Configs;
using Kirel.Identity.Server.API.Controllers;
using Kirel.Identity.Server.API.Extensions;
using Kirel.Identity.Server.API.Filters;
using Kirel.Identity.Server.API.Handlers;
using Kirel.Identity.Server.Core.Extensions;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.Infrastructure.Contexts;
using Kirel.Identity.Server.Infrastructure.Extensions;
using Kirel.Identity.Server.Infrastructure.Managers;
using Kirel.Identity.Server.Infrastructure.Models;
using Kirel.Identity.Server.Infrastructure.Providers;
using Kirel.Identity.Server.Infrastructure.Shared.Extensions;
using Kirel.Identity.Server.Infrastructure.Shared.Models;
using Kirel.Identity.Server.Jwt.Shared;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

var jwtConfig = builder.Configuration.GetSection("jwt").Get<JwtAuthenticationConfig>();
var dbConfig = builder.Configuration.GetSection("dbConfig").Get<DbConfig>();
var maintenanceConfig = builder.Configuration.GetSection("maintenance").Get<MaintenanceConfig>();
var dataSeedConfig = builder.Configuration.GetSection("DataSeeding").Get<IdentityDataSeedConfig>();
var registrationDisabled = builder.Configuration.GetValue<bool>("RegistrationDisabled");
var apiKeys = builder.Configuration.GetSection("APIKeys").Get<ApiKeysList>();
var smsSenderConfig = builder.Configuration.GetSection("MainSms").Get<MainSmsSenderConfig>();
var codeTokenConfig = builder.Configuration.GetSection("DisposableCodes").Get<DisposableCodesConfig>();

//To disable controller add him here like disabledControllers.Add(typeof(RegistrationController));
var disabledControllers = new DisabledControllerTypes();
if (registrationDisabled) disabledControllers.Add(typeof(RegistrationController));


builder.Services.AddSingleton(codeTokenConfig);
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Tokens.ProviderMap["Code provider"] = new TokenProviderDescriptor(typeof(DisposableCodeTokenProvider));
});
builder.Services.AddSingleton<DisposableCodeTokenProvider>();

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

builder.Services.AddSmsAuthentication<MainSmsSenderManager>(smsSenderConfig);
// Add ASP.NET authentication configuration
builder.Services.AddAuthenticationConfiguration(jwtConfig, apiKeys);
builder.Services.AddClaimBasedAuthorization();

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
await app.MaintenanceAsync<Guid, User, Role, UserRole, UserClaim, RoleClaim>(maintenanceConfig);
// Apply users and roles data seeding
await app.UsersAndRolesDataSeedAsync<Guid, User, Role, UserRole, UserClaim, RoleClaim>(dataSeedConfig);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DevCorsPolicy");
//app.UseHttpsRedirection();

// for now we use static index html to use stoplight/elements OpenAPI documentation on address /docs
// Save order UseDefaultFiles() - First.
app.UseDefaultFiles();
app.UseStaticFiles();

// Save order, UseAuthentication() - First.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Middleware that handle all exceptions throws and set response code depends on exception type
app.UseMiddleware<KirelErrorHandlerMiddleware>();

app.Run();