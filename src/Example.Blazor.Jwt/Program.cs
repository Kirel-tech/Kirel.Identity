using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Example.Blazor.Jwt;
using Kirel.Identity.Client.Blazor.Services;
using Kirel.Identity.Client.Interfaces;
using Kirel.Identity.Client.Jwt.Handlers;
using Kirel.Identity.Client.Jwt.Options;
using Kirel.Identity.Client.Jwt.Providers;
using Kirel.Identity.Client.Jwt.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

// From Example.API project /Properties/launchSettings.json profiles -> Example
const string identityUriStr = "https://localhost:7264/";
var identityUri = new Uri(identityUriStr);

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

// Add client token storage
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IClientTokenService, KirelBlazoredClientTokenService>();

// Add http client factory instance with default http client for fetch data example
builder.Services.AddHttpClient(string.Empty, hc => hc.BaseAddress = new Uri("https://localhost:7005"));

// Add Identity http client with JWT Authorization handler to factory
builder.Services.AddScoped<KirelJwtHttpClientAuthorizationHandler>();
builder.Services.AddHttpClient("IdentityAuthorized", hc => hc.BaseAddress = identityUri)
    .AddHttpMessageHandler<KirelJwtHttpClientAuthorizationHandler>();

// Add JWT authentication service with configuration for working with Example project API endpoint
builder.Services.AddScoped<IClientAuthenticationService, KirelClientJwtAuthenticationService>();
builder.Services.Configure<KirelClientJwtAuthenticationOptions>(options =>
{
    options.BaseUrl = identityUriStr;
    options.RelativeUrl = "authentication/jwt"; // From Example project /Controllers/ExJwtAuthenticationController Route Attribute
});

// Add JWT Authentication state provider
builder.Services.AddScoped<KirelJwtTokenAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<KirelJwtTokenAuthenticationStateProvider>());

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();