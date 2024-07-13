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
using Server;
using Server.CommandHandlers;
using Server.Models.DTO;

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
        services.AddSingleton(configuration);

        // Configure DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Register repository
        services.RegisterRepositories();
        services.RegisterServices();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddSingleton<ICommandHandlerRegistry, CommandHandlerRegistry>();
        services.AddHostedService<ServerHostedService>();
    })
    .Build();

await host.RunAsync();


