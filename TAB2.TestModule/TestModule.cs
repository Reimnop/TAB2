using Brigadier.NET;
using Brigadier.NET.Builder;
using Brigadier.NET.Context;
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
    
    private readonly ILog log = LogManager.GetLogger("testmodule");
    private TestData testData = new TestData();

    private IBotInstance instance;
    
    public override void Initialize(IBotInstance instance)
    {
        this.instance = instance;
        
        instance.DataManager.RegisterData(ModuleId, testData);

        log.Info("Hello World from Test Module!");
    }

    public override async Task OnReady()
    {
        log.Info("Test Module is ready!");

        await instance.Client.SetStatusAsync(UserStatus.DoNotDisturb);
    }

    public override IEnumerator<CommandBuilder> OnCommandRegister()
    {
        yield return new CommandBuilder("ping")
            .WithDescription("Ping");
    }

    public override async Task OnSlashCommandExecuted(SocketSlashCommand slashCommand)
    {
    }

    public override Task OnMessageReceived(SocketMessage message)
    {
        log.Info($"{message.Author.Username} sent '{message.Content}'");
        return Task.CompletedTask;
    }
}