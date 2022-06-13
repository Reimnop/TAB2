using TAB2.Api.Events;

namespace TAB2.Module;

public class Module
{
    public ModuleInfo Info { get; set; }
    public ModuleEventBus EventBus { get; set; }

    public Module(ModuleInfo info, ModuleEventBus eventBus)
    {
        Info = info;
        EventBus = eventBus;
    }
}