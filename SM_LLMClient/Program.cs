
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SM_LLMClient;
using SM_LLMClient.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Configure HttpClient to point to the server API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:7000/")
});

// Register PII Detection service
builder.Services.AddScoped<EnhancedPiiDetector>();

Console.WriteLine("Base Address: " + builder.HostEnvironment.BaseAddress);

await builder.Build().RunAsync();
