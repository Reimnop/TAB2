using Discord.WebSocket;
using TAB2.Api.Command;

namespace TAB2.Command;

public class SlashCommandContext : ICommandContext
{
    private readonly SocketSlashCommand slashCommand;

    public SlashCommandContext(SocketSlashCommand slashCommand)
    {
        this.slashCommand = slashCommand;
    }
    
    public async Task RespondAsync(string message)
    {
        await slashCommand.RespondAsync(message);
    }
}