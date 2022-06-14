using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using TAB2.Api.Module;

namespace TAB2.Module;

public static class ModuleLoader
{
    public static bool TryLoadModule(
        Assembly assembly,
        [MaybeNullWhen(false)] out BaseModule baseModule, 
        [MaybeNullWhen(false)] out ModuleEntryAttribute attribute)
    {
        IEnumerable<Type> types = assembly.GetTypes()
            .Where(x => !x.IsInterface && !x.IsAbstract && typeof(BaseModule).IsAssignableFrom(x));

        baseModule = null;
        attribute = null;
        
        foreach (Type type in types)
        {
            if (Attribute.GetCustomAttribute(type, typeof(ModuleEntryAttribute)) is ModuleEntryAttribute moduleEntryAttribute)
            {
                baseModule = (BaseModule) Activator.CreateInstance(type);
                attribute = moduleEntryAttribute;
                return true;
            }
        }

        return false;
    }
}