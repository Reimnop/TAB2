using log4net;
using TAB2.Api.Events;
using TAB2.Api.Module;

namespace TAB2.TestModule;

[ModuleEntry("TestModule", "testmodule", "1.0.0")]
public class TestModule : IModule
{
    private ILog log;
    
    public void Initialize(ModuleEventBus eventBus)
    {
        log = LogManager.GetLogger("testmodule");
        log.Info("Hello World from Test Module!");

        eventBus.OnReady += () =>
        {
            log.Info("Bot is ready!");
        };
        
        eventBus.OnMessageReceived += message =>
        {
            log.Info($"{message.Author} sent '{message.Content}'");
        };
    }
}