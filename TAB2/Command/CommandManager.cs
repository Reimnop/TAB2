using TAB2.Api.Command;

namespace TAB2.Command;

public class CommandManager
{
    private readonly Dictionary<string, DiscordCommand> commands = new Dictionary<string, DiscordCommand>();

    public void RegisterCommand(DiscordCommand command)
    {
        commands.Add(command.Name, command);
    }

    public Task RunCommand(string command, ICommandContext context)
    {
        if (commands.TryGetValue(command, out DiscordCommand? discordCommand))
        {
            if (discordCommand.ExecutesTaskDelegate != null)
            {
                return discordCommand.ExecutesTaskDelegate(context);
            }
        }
        
        return Task.CompletedTask;
    }
}