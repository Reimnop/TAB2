using TAB2.Api.Command;

namespace TAB2.Command;

public class CommandManager
{
    private readonly Dictionary<string, DiscordCommandInfo> commands = new Dictionary<string, DiscordCommandInfo>();

    public void RegisterCommand(DiscordCommandInfo commandInfo)
    {
        commands.Add(commandInfo.Name, commandInfo);
    }

    public Task RunCommand(string command, ICommandContext context)
    {
        if (commands.TryGetValue(command, out DiscordCommandInfo? discordCommand))
        {
            if (discordCommand.ExecutesTaskDelegate != null)
            {
                return discordCommand.ExecutesTaskDelegate(context);
            }
        }
        
        return Task.CompletedTask;
    }
}