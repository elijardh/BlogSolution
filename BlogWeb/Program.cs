using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlogWeb;
using BlogWeb.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<DialogService>();
builder.Services.AddTransient<HomeServices>();
builder.Services.AddTransient<AuthenticationService>();
builder.Services.AddSingleton<UserService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5069/") });

await builder.Build().RunAsync();

