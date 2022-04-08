using Discord;
using Discord.WebSocket;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using TAB2.Commands;
using TAB2.Configuration;

namespace TAB2;

public class BotMain : IDisposable
{
    private readonly DiscordSocketClient client;
    private readonly CommandList commands;

    private readonly ILog log;

    public BotMain()
    {
        log = LogManager.GetLogger("Discord");
        
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
        _ = Task.Run(() => commands.ExecuteCommand(command));
        return Task.CompletedTask;
    }

    private Task ClientOnLog(LogMessage msg)
    {
        switch (msg.Severity)
        {
            case LogSeverity.Debug:
                log.Debug(msg.Message);
                break;
            case LogSeverity.Info:
                log.Info(msg.Message);
                break;
            case LogSeverity.Warning:
                log.Warn(msg.Message);
                break;
            case LogSeverity.Error:
                log.Error(msg.Message, msg.Exception);
                break;
            case LogSeverity.Critical:
                log.Fatal(msg.Message, msg.Exception);
                break;
        }
        
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        client.Dispose();
    }
}