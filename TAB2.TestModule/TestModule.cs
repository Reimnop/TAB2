using Brigadier.NET;
using Brigadier.NET.Builder;
using Discord.WebSocket;
using log4net;
using TAB2.Api.Command;
using TAB2.Api.Module;

namespace TAB2.TestModule;

[ModuleEntry("Test Module", "testmodule", "1.0.0")]
public class TestModule : BaseModule
{
    private readonly ILog log = LogManager.GetLogger("testmodule");
    
    public override void Initialize()
    {
        log.Info("Hello World from Test Module!");
    }

    public override void OnReady()
    {
        log.Info("Test Module is ready!");
    }

    public override void OnCommandRegister(CommandDispatcher<CommandSource> dispatcher)
    {
        dispatcher.Register(a => a.Literal("helloworld")
            .Executes(context =>
            {
                context.Source.Channel.SendMessageAsync("Hello world!").Wait();
                return 1;
            })
        );
    }

    public override void OnMessageReceived(SocketMessage message)
    {
        log.Info($"{message.Author.Username} sent '{message.Content}'");
    }
}