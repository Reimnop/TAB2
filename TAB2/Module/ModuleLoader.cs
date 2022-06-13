using System.Reflection;
using TAB2.Api.Module;

namespace TAB2.Module;

public class ModuleLoader
{
    private readonly Assembly assembly;
    
    public ModuleLoader(Assembly assembly)
    {
        this.assembly = assembly;
    }

    public ModuleInfo? GetModuleInfo()
    {
        IEnumerable<Type> types = assembly.GetTypes()
            .Where(x => !x.IsInterface && !x.IsAbstract && typeof(IModule).IsAssignableFrom(x));
        
        foreach (Type type in types)
        {
            if (Attribute.GetCustomAttribute(type, typeof(ModuleEntryAttribute)) is ModuleEntryAttribute moduleEntryAttribute)
            {
                IModule module = (IModule) Activator.CreateInstance(type);
                return new ModuleInfo(module, moduleEntryAttribute);
            }
        }

        return null;
    }
}