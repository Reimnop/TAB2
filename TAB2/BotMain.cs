using Discord;
using Discord.WebSocket;
using log4net;
using TAB2.Module;

namespace TAB2;

public class BotMain : IDisposable
{
    private readonly ILog log;
    
    private readonly DiscordSocketClient client;
    private readonly ModuleManager moduleManager;

    public BotMain()
    {
        log = LogManager.GetLogger("Discord");
        
        DiscordSocketConfig config = new DiscordSocketConfig();
        config.DefaultRetryMode = RetryMode.AlwaysRetry;

        client = new DiscordSocketClient(config);

        moduleManager = new ModuleManager();
    }

    public async Task Run(string token)
    {
        moduleManager.LoadModules("Modules");
        moduleManager.InitializeModules();

        client.Log += ClientOnLog;
        
        client.Ready += ClientOnReady;
        client.MessageReceived += ClientOnMessageReceived;

        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();

        await Task.Delay(Timeout.Infinite);
    }

    private Task ClientOnReady()
    {
        return moduleManager.RunOnAllModules(module =>
        {
            module.EventBus.RaiseReadyEvent();
        });
    }

    private Task ClientOnMessageReceived(SocketMessage message)
    {
        return moduleManager.RunOnAllModules(module =>
        {
            module.EventBus.RaiseMessageReceivedEvent(message);
        });
    }

    private Task ClientOnLog(LogMessage msg)
    {
        switch (msg.Severity)
        {
            case LogSeverity.Debug:
                log.Debug(msg.Message, msg.Exception);
                break;
            case LogSeverity.Info:
                log.Info(msg.Message, msg.Exception);
                break;
            case LogSeverity.Warning:
                log.Warn(msg.Message, msg.Exception);
                break;
            case LogSeverity.Error:
                log.Error(msg.Message, msg.Exception);
                break;
            case LogSeverity.Critical:
                log.Fatal(msg.Message, msg.Exception);
                break;
            case LogSeverity.Verbose:
                log.Debug(msg.Message, msg.Exception);
                break;
        }
        
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        client.Dispose();
    }
}