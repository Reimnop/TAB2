using Discord;
using Discord.WebSocket;

namespace TAB2.Commands;

public class HelloWorldCommand : Command
{
    public override SlashCommandBuilder GetSlashCommand()
    {
        SlashCommandBuilder commandBuilder = new SlashCommandBuilder()
            .WithName("helloworld")
            .WithDescription("Say con c!");
        return commandBuilder;
    }
    
    public override async Task ExecuteCommand(SocketSlashCommand command)
    {
        await command.RespondAsync("Hello world!");
    }

    public override void Dispose()
    {
    }
}