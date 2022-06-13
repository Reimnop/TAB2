using TAB2.Api.Events;

namespace TAB2.Api.Module;

public interface IModule
{
    public void Initialize(ModuleEventBus eventBus);
}