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

    public bool GetArgument<T>(string name, out T? value)
    {
        if (arguments.TryGetValue(name, out object? uncastedValue))
        {
            if (uncastedValue is T castedValue)
            {
                value = castedValue;
                return true;
            }
        }

        value = default;
        return false;
    }

    public async Task RespondAsync(string message)
    {
        await slashCommand.RespondAsync(message);
    }
}