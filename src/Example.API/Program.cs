using System.Reflection;
using Example.API.Contexts;
using Example.API.Models;
using Example.API.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Kirel.Identity.Core.Models;
using Kirel.Identity.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add kirel based identity db context
builder.Services.AddDbContext<ExKirelIdentityContext>(options =>
    options.UseSqlite("Filename=example.db"));

//Add identity framework stuff
builder.Services.AddIdentity<ExUser, ExRole>().AddEntityFrameworkStores<ExKirelIdentityContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    // User settings.
    options.User.RequireUniqueEmail = true;
});

// Add dto validators. Configured in Validators.
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

//Add AutoMapper. Class <--> Dto mappings. Configured in Mappings.
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Add kirel based identity management services
builder.Services.AddScoped<ExAuthenticationService>();
builder.Services.AddScoped<ExAuthorizedUserService>();
builder.Services.AddScoped<ExRegistrationService>();
builder.Services.AddScoped<ExRoleService>();
builder.Services.AddScoped<ExUserService>();

// add kirel based jwt tokens generation service
builder.Services.AddScoped<ExJwtTokenService>();

// Add kirel authentication/authorization options
var authOptions = new KirelAuthOptions()
{
    AccessLifetime = 5,
    Audience = "ExampleClient",
    Issuer = "ExampleServer",
    Key = "SomeSuperSecretKey123",
    RefreshLifetime = 3600
};
builder.Services.AddSingleton(authOptions);

// Add ASP.NET authentication configuration
builder.Services.AddAuthentication(option =>
    {
        // Fixing 404 error when adding an attribute Authorize to controller
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = authOptions.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = authOptions.GetSymmetricSecurityKey(authOptions.Key),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure swagger
builder.Services.AddSwaggerGen(c =>
{
    //Add swagger xml docs
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kirel example identity API", Version = "v1" });
    //Set the comments path for the swagger json and ui.
    List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
    xmlFiles.ForEach(xmlFile => c.IncludeXmlComments(xmlFile));
    // Add JWT token authorization support
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()  
    {  
        Name = "Authorization",  
        Type = SecuritySchemeType.ApiKey,  
        Scheme = "Bearer",  
        BearerFormat = "JWT",  
        In = ParameterLocation.Header,  
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",  
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement  
    {  
        {  
            new OpenApiSecurityScheme  
            {  
                Reference = new OpenApiReference  
                {  
                    Type = ReferenceType.SecurityScheme,  
                    Id = "Bearer"  
                }  
            },  
            new string[] {}
        }  
    });
});

//it is necessary for requests from the host locale,
//to work without a signed certificate, and we allow the use of any HTTP methods and hiders
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

// Initialize database
await SeedDb.Initialize(app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevCorsPolicy");
}

app.UseHttpsRedirection();

// Save order UseAuthentication() - First.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Middleware that handle all exceptions throws and set response code depends on exception type
app.UseMiddleware<KirelErrorHandlerMiddleware>();
app.Run();