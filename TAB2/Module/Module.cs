using TAB2.Api.Events;
using TAB2.Api.Module;

namespace TAB2.Module;

public class Module
{
    public IModule EntryPoint { get; set; }
    public ModuleEntryAttribute Attribute { get; set; }
    public ModuleEventBus EventBus { get; set; }

    public Module(IModule entryPoint, ModuleEntryAttribute attribute, ModuleEventBus eventBus)
    {
        EntryPoint = entryPoint;
        Attribute = attribute;
        EventBus = eventBus;
    }
}