using System.Reflection;
using Discord;
using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.Interactions;
using Discord.WebSocket;

namespace You7.Services
{
    public class CommandHandler : DiscordClientService
    {
        private readonly IServiceProvider _provider;
        private readonly InteractionService _service;
        private readonly IConfiguration _config;

        public CommandHandler(DiscordSocketClient client, ILogger<DiscordClientService> logger, IServiceProvider provider,
                                InteractionService service, IConfiguration config) : base(client, logger)
        {
            _provider = provider;
            _service = service;
            _config = config;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Client.InteractionCreated += HandleInteraction;
            // Process the command execution results
            _service.SlashCommandExecuted += SlashCommandExecuted;
            _service.ContextCommandExecuted += ContextCommandExecuted;
            _service.ComponentCommandExecuted += ComponentCommandExecuted;

            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
            await Client.WaitForReadyAsync(cancellationToken);

            await _service.RegisterCommandsGloballyAsync();
        }

        private Task ComponentCommandExecuted(ComponentCommandInfo commandInfo, IInteractionContext context, IResult result)
        {
            if (!result.IsSuccess)
                Console.WriteLine("error");
            return Task.CompletedTask;
        }

        private Task ContextCommandExecuted(ContextCommandInfo commandInfo, IInteractionContext context, IResult result)
        {
            if (!result.IsSuccess)
                Console.WriteLine("error");
            return Task.CompletedTask;
        }

        private Task SlashCommandExecuted(SlashCommandInfo commandInfo, IInteractionContext context, IResult result)
        {
            if (!result.IsSuccess)
                Console.WriteLine("error");
            return Task.CompletedTask;
        }

        private async Task HandleInteraction(SocketInteraction arg)
        {
            try
            {
                // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules
                var context = new SocketInteractionContext(Client, arg);
                await _service.ExecuteCommandAsync(context, _provider);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Exception occurred whilst attempting to handle interaction.");

                if (arg.Type == InteractionType.ApplicationCommand)
                {
                    var msg = await arg.GetOriginalResponseAsync();
                    await msg.DeleteAsync();
                }
            }
        }
    }
}
