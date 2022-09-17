using Discord;
using Discord.WebSocket;
using log4net;
using TAB2.Api;
using TAB2.Api.Command;
using TAB2.Api.Module;

namespace TAB2.TestModule;

[ModuleEntry("Test Module", ModuleId, "1.0.0")]
public class TestModule : BaseModule
{
    private const string ModuleId = "testmodule";
    
    private readonly ILog log = LogManager.GetLogger(ModuleId);

    private IBotInstance instance;
    
    public override void Initialize(IBotInstance instance)
    {
        this.instance = instance;

        log.Info("Hello World from Test Module!");
    }

    public override async Task OnReady()
    {
        log.Info("Test Module is ready!");

        await instance.Client.SetStatusAsync(UserStatus.DoNotDisturb);
    }

    public override IEnumerator<DiscordCommandInfo> OnCommandRegister()
    {
        yield return new DiscordCommandInfo()
            .WithName("ping")
            .WithDescription("Ping")
            .Executes(PingCommand);
        
        yield return new DiscordCommandInfo()
            .WithName("enchart")
            .WithDescription("flushed")
            .Executes(EnchartFlushedCommand);

        yield return new DiscordCommandInfo()
            .WithName("say")
            .WithDescription("what the fuck")
            .AddStringArgument("message", "shit wtf")
            .Executes(SayCommand);
    }

    private async Task PingCommand(ICommandContext context)
    {
        await context.RespondAsync($"Pong! :ping_pong:\nLatency: {instance.Client.Latency}ms");
    }

    private async Task EnchartFlushedCommand(ICommandContext context)
    {
        await context.RespondAsync("enchy wenchy :squeee:");
    }

    private async Task SayCommand(ICommandContext context)
    {
        await context.DeferAsync();
        await Task.Delay(2000);
        if (context.GetArgument("message", out string? message))
        {
            await context.RespondAsync($"{context.User.Mention} said '{message}'");
        }
    }

    public override Task OnMessageReceived(SocketMessage message)
    {
        log.Info($"{message.Author.Username} sent '{message.Content}'");
        return Task.CompletedTask;
    }
}