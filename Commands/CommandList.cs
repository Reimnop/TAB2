using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace TAB2.Commands;

public class CommandList : IDisposable
{
    private readonly Dictionary<string, int> commandIndices = new();
    private readonly List<Command> commands = new();
    
    private readonly List<Type> registeredCommandTypes = new();
    
    private readonly DiscordSocketClient client;
    
    private readonly IServiceCollection serviceCollection; 

    public CommandList(DiscordSocketClient client, IServiceCollection services)
    {
        this.client = client;

        serviceCollection = services
            .AddSingleton(this)
            .AddSingleton(client);

        RegisterCommands();
        BuildCommands();

        client.JoinedGuild += ClientOnJoinedGuild;
        client.MessageReceived += ClientOnMessageReceived;
    }

    private async Task ClientOnMessageReceived(SocketMessage arg)
    {
        if (arg.Author.IsBot)
        {
            return;
        }

        if (arg.Channel is SocketGuildChannel channel && arg.Content == "@@tab2ForceRefreshCmds")
        {
            await RefreshCommands(channel.Guild);
            await arg.Channel.SendMessageAsync("Force refresh requested!");
        }
    }

    private void RegisterCommands()
    {
        RegisterCommand<RefreshCommandsCommand>();
        RegisterCommand<ConfigCommand>();
        RegisterCommand<PingCommand>();
        RegisterCommand<StatsCommand>();
        RegisterCommand<ChatCommand>();
    }

    private async Task ClientOnJoinedGuild(SocketGuild guild)
    {
        foreach (var command in commands)
        {
            if (command.ShouldAddToGuild(guild))
            {
                await guild.CreateApplicationCommandAsync(command.GetSlashCommand().Build());
            }
        }
    }

    public async Task RefreshCommands(SocketGuild guild)
    {
        var applicationCommands = new List<ApplicationCommandProperties>();
        foreach (var command in commands)
        {
            if (command.ShouldAddToGuild(guild))
            {
                applicationCommands.Add(command.GetSlashCommand().Build());
            }
        }

        await guild.BulkOverwriteApplicationCommandAsync(applicationCommands.ToArray());
    }

    private void BuildCommands()
    {
        using var serviceProvider = serviceCollection.BuildServiceProvider();
        for (int i = 0; i < registeredCommandTypes.Count; i++)
        {
            Command command = (Command)serviceProvider.GetRequiredService(registeredCommandTypes[i]);
            commands.Add(command);
            commandIndices.Add(command.GetSlashCommand().Name, i);
        }
    }

    private void RegisterCommand<T>() where T : Command
    {
        serviceCollection.AddSingleton<T>();
        registeredCommandTypes.Add(typeof(T));
    }

    public async Task ExecuteCommand(SocketSlashCommand command)
    {
        if (commandIndices.TryGetValue(command.CommandName, out var i))
        {
            await commands[i].ExecuteCommand(command);
            return;
        }

        await command.RespondAsync("Error: How did you even run a non-existent slash command?", ephemeral: true);
    }
    
    public void Dispose()
    {
        client.JoinedGuild -= ClientOnJoinedGuild;
        client.MessageReceived -= ClientOnMessageReceived;
        
        foreach (var command in commands)
        {
            command.Dispose();
        }
    }
}