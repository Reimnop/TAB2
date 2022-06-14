using log4net;
using TAB2.Api.Events;
using TAB2.Api.Module;

namespace TAB2.TestModule;

[ModuleEntry("TestModule", "testmodule", "1.0.0")]
public class TestModule : IModule
{
    private readonly ILog log = LogManager.GetLogger("testmodule");
    
    public void Initialize(ModuleEventBus eventBus)
    {
        log.Info("Hello World from Test Module!");
    }
}