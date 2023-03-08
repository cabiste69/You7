using System.Diagnostics;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using You7.Services;


string settingsFile = Debugger.IsAttached ? "appsettings.Development.json" : "appsettings.json";

var builder = new HostBuilder()
    .ConfigureAppConfiguration(x =>
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(settingsFile, false)
            .Build();
        x.AddConfiguration(configuration);
    })
    .ConfigureLogging(x =>
    {
        x.AddConsole();
        x.SetMinimumLevel(LogLevel.Debug);
    })
    .ConfigureDiscordHost((context, config) =>
    {
        config.SocketConfig = new DiscordSocketConfig
        {
            LogLevel = Discord.LogSeverity.Debug,
            AlwaysDownloadUsers = false,
            MessageCacheSize = 20,
            GatewayIntents = Discord.GatewayIntents.All
        };
        config.Token = context.Configuration["Token"];
    })
    .UseInteractionService((contect, config) =>
    {
        config.LogLevel = Discord.LogSeverity.Debug;
        config.DefaultRunMode = Discord.Interactions.RunMode.Async;
    })
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<CommandHandler>();
        services.AddHostedService<BotStatuService>();
    })
    .UseConsoleLifetime();

var host = builder.Build();


using (host) await host.RunAsync();
