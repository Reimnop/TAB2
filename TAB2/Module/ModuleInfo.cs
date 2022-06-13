using TAB2.Api.Module;

namespace TAB2.Module;

public class ModuleInfo
{
    public IModule EntryPoint { get; set; }
    public ModuleEntryAttribute Attribute { get; set; }

    public ModuleInfo(IModule entry, ModuleEntryAttribute attribute)
    {
        EntryPoint = entry;
        Attribute = attribute;
    }
}