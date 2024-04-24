using BlazorWebApp.Client;
using BlazorWebApp.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddSingleton<IUserContext, UserContext>();
builder.Services.AddSingleton<ICookieStorageService, CookieStorageService>();

await builder.Build().RunAsync();
