using Discord;
using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.WebSocket;

namespace You7.Services
{
    public class BotStatuService : DiscordClientService
    {
        public BotStatuService(DiscordSocketClient client, ILogger<DiscordClientService> logger) : base(client, logger)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Client.WaitForReadyAsync(stoppingToken);
            Logger.LogInformation("Client is ready!");
            await Client.SetActivityAsync(new Game("Mee6 suffering >:)", ActivityType.Watching));
        }
    }
}
