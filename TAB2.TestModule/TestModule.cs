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

    public override Task OnReady()
    {
        log.Info("Test Module is ready!");
        return Task.CompletedTask;
    }

    public override Task OnCommandRegister(CommandDispatcher<CommandSource> dispatcher)
    {
        dispatcher.Register(a => a.Literal("say")
            .Then(b => b.Argument("message", Arguments.String())
                .Executes(context =>
                {
                    CommandSource source = context.Source;
                    source.Channel.SendMessageAsync($"{source.User.Mention} said '{context.GetArgument<string>("message")}'");
                    return 1;
                })
            )
        );
        return Task.CompletedTask;
    }

    public override Task OnMessageReceived(SocketMessage message)
    {
        log.Info($"{message.Author.Username} sent '{message.Content}'");
        return Task.CompletedTask;
    }
}