namespace TAB2.Api.Command;

public delegate Task CommandExecutesTaskDelegate(ICommandContext commandContext);

public class DiscordCommand
{
    public string Name { get; }
    public string Description { get; set; }
    public CommandExecutesTaskDelegate? ExecutesTaskDelegate { get; set; }

    public DiscordCommand(string name)
    {
        Name = name;
    }

    public DiscordCommand WithDescription(string description)
    {
        Description = description;
        return this;
    }

    public DiscordCommand Executes(CommandExecutesTaskDelegate executesTaskDelegate)
    {
        ExecutesTaskDelegate = executesTaskDelegate;
        return this;
    }
}