using Discord;
using Discord.WebSocket;
using TAB2.Api.Command;

namespace TAB2.Command;

public class SlashCommandManager
{
    private readonly Dictionary<string, DiscordCommandInfo> commands = new Dictionary<string, DiscordCommandInfo>();
    private readonly DiscordSocketClient client;

    public SlashCommandManager(DiscordSocketClient client)
    {
        this.client = client;
    }

    public async Task RegisterCommand(DiscordCommandInfo commandInfo)
    {
        foreach (SocketGuild guild in client.Guilds)
        {
            SlashCommandBuilder commandBuilder = new SlashCommandBuilder()
                .WithName(commandInfo.Name)
                .WithDescription(commandInfo.Description);
            
            commandInfo.Arguments.ForEach(x => SetupArgument(commandBuilder, x));
            
            await guild.CreateApplicationCommandAsync(commandBuilder.Build());
        }
        
        commands.Add(commandInfo.Name, commandInfo);
    }

    private void SetupArgument(SlashCommandBuilder builder, ArgumentInfo info)
    {
        if (info is IntArgumentInfo)
        {
            builder.AddOption(info.Name, ApplicationCommandOptionType.Integer, info.Description, info.IsRequired);
            return;
        }
        
        if (info is DoubleArgumentInfo)
        {
            builder.AddOption(info.Name, ApplicationCommandOptionType.Number, info.Description, info.IsRequired);
            return;
        }
        
        if (info is StringArgumentInfo)
        {
            builder.AddOption(info.Name, ApplicationCommandOptionType.String, info.Description, info.IsRequired);
            return;
        }
        
        if (info is EnumArgumentInfo enumArgumentInfo)
        {
            builder.AddOption(info.Name, ApplicationCommandOptionType.Integer, info.Description, info.IsRequired, 
                choices: enumArgumentInfo.Options
                    .Select(x => new ApplicationCommandOptionChoiceProperties
                    {
                        Name = x.Item2,
                        Value = x.Item1
                    })
                    .ToArray());
            return;
        }
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