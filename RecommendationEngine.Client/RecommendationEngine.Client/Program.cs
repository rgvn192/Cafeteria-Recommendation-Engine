// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RecommendationEngine.Client;
using RecommendationEngine.Client.Interfaces;
using RecommendationEngine.Client.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IChefService, ChefService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<Client>();
        services.AddLogging(configure => configure.AddConsole());
    })
    .Build();

var client = host.Services.GetRequiredService<Client>();
await client.StartClientAsync();
await host.RunAsync();