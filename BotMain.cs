using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using TAB2.Commands;
using TAB2.Configuration;

namespace TAB2;

public class BotMain : IDisposable
{
    private readonly DiscordSocketClient client;
    private readonly CommandList commands;

    private readonly ConfigService configService;

    public BotMain()
    {
        DiscordSocketConfig config = new DiscordSocketConfig();
        config.GatewayIntents = GatewayIntents.All;
        config.AlwaysDownloadUsers = true;
        client = new DiscordSocketClient(config);

        var services = new ServiceCollection()
            .AddSingleton<ConfigService>();
        
        commands = new CommandList(client, services);
    }

    public async Task Run(string token)
    {
        client.Log += ClientOnLog;
        client.SlashCommandExecuted += ClientOnSlashCommandExecuted;

        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();
        
        await Task.Delay(Timeout.Infinite);
    }

    private Task ClientOnSlashCommandExecuted(SocketSlashCommand command)
    {
        return Task.Run(() => commands.ExecuteCommand(command));
    }

    private Task ClientOnLog(LogMessage arg)
    {
        Console.WriteLine(arg.ToString());
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        client.Dispose();
    }
}