using Discord;
using Discord.WebSocket;

namespace TAB2.Commands;

public class RefreshCommandsCommand : Command
{
    private readonly CommandList commandList;

    public RefreshCommandsCommand(CommandList commandList)
    {
        this.commandList = commandList;
    }

    public override bool ShouldAddToGuild(SocketGuild guild)
    {
        return true;
    }

    public override SlashCommandBuilder GetSlashCommand()
    {
        SlashCommandBuilder commandBuilder = new SlashCommandBuilder()
            .WithName("refresh")
            .WithDescription("Refreshes all commands.");
        return commandBuilder;
    }
    
    public override async Task ExecuteCommand(SocketSlashCommand command)
    {
        if (command.Channel is SocketGuildChannel guildChannel)
        {
            await commandList.RefreshCommands(guildChannel.Guild);
            await command.RespondAsync("Commands refreshed!");
        }
        else
        {
            await command.RespondAsync("This command must be run in a guild!");
        }
    }

    public override void Dispose()
    {
    }
}