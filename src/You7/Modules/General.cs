using Discord.Interactions;

namespace You7.Modules;

/// <summary>
/// For funny commands. <br/>
/// memes from reddit / image distortion / banning those i disagree with etc...
/// </summary>
public class General : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("ping", "simple command to make sure the bot is active")]
    public async Task Ping()
    {
        await RespondAsync("Pong!");
    }
}