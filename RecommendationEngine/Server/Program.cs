// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RecommendationEngine.Data;
using Server.Extentions;
using RecommendationEngine.Data.Extentions;
using RecommendationEngine.Data.Entities;
using Server.Services;
using Server.Interface;

Console.WriteLine("Hello, World!");

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory());
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        // Register IConfiguration
        var configuration = context.Configuration;
        services.AddSingleton<IConfiguration>(configuration);

        // Configure DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Register repository
        services.RegisterRepositories();
        services.RegisterServices();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    })
    .Build();

await CheckRepo(host.Services);
async Task CheckRepo(IServiceProvider services)
{
    using (var scope = services.CreateScope())
    {
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

        var user = new User
        {
            Email = "rgvn192@gmail.com",
            RoleId = 2,
            Name = "Rgvn"
        };
        await userService.Add<User>(user);

        Console.WriteLine($"Received and stored notification for user");
    }
}
