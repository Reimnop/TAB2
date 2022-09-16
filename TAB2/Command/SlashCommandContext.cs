using Discord.WebSocket;
using TAB2.Api.Command;

namespace TAB2.Command;

public class SlashCommandContext : ICommandContext
{
    private readonly SocketSlashCommand slashCommand;
    private readonly Dictionary<string, object> arguments;

    public SlashCommandContext(SocketSlashCommand slashCommand)
    {
        this.slashCommand = slashCommand;
        arguments = slashCommand.Data.Options.ToDictionary(x => x.Name, x => x.Value);
    }

    public T? GetArgument<T>(string name)
    {
        if (arguments.TryGetValue(name, out object? value))
        {
            
        }

        return null;
    }

    public async Task RespondAsync(string message)
    {
        await slashCommand.RespondAsync(message);
    }
}