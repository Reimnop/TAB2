using System.Reflection;
using System.Text;
using log4net;
using TAB2.Api.Events;
using TAB2.Api.Module;

namespace TAB2.Module;

public delegate void ModuleRunDelegate(Module module);

public class ModuleManager
{
    private readonly ILog log;
    private readonly Dictionary<string, Module> loadedModules;

    public ModuleManager()
    {
        log = LogManager.GetLogger("ModuleManager");
        loadedModules = new Dictionary<string, Module>();
    }

    public void LoadModules(string directory)
    {
        List<Module> modules = new List<Module>();
        
        FileInfo[] files = new DirectoryInfo(directory).GetFiles();
        IEnumerable<FileInfo> assemblyFiles = files
            .Where(x => x.Extension == ".dll");
        foreach (FileInfo assemblyFile in assemblyFiles)
        {
            Assembly assembly = Assembly.LoadFile(assemblyFile.FullName);
            Module? module = LoadModule(assembly);

            if (module == null)
            {
                log.Warn($"Could not load module from assembly '{assembly}'!");
                continue;
            }
            
            modules.Add(module);
        }
        
        LogDiscoveredModules(modules);

        foreach (Module module in modules)
        {
            module.EntryPoint.Initialize(module.EventBus);
            loadedModules.Add(module.Attribute.Id, module);
        }
    }

    private void LogDiscoveredModules(ICollection<Module> modules)
    {
        StringBuilder text = new StringBuilder($"Discovered {modules.Count} module(s)\n");
        foreach (Module module in modules)
        {
            text.Append($"    - {module.Attribute.Name} (id: '{module.Attribute.Id}', version: {module.Attribute.Version})\n");
        }
        log.Info(text.ToString());
    }

    private Module? LoadModule(Assembly assembly)
    {
        if (!ModuleLoader.TryLoadModule(assembly, out IModule? entryPoint, out ModuleEntryAttribute? attribute))
        {
            return null;
        }
        
        ModuleEventBus eventBus = new ModuleEventBus();
        return new Module(entryPoint, attribute, eventBus);
    }

    public void RunOnAllModules(ModuleRunDelegate moduleRunDelegate)
    {
        foreach (Module module in loadedModules.Values)
        {
            moduleRunDelegate(module);
        }
    }

    public bool TryRunOnModule(string id, ModuleRunDelegate moduleRunDelegate)
    {
        if (!loadedModules.TryGetValue(id, out Module? module))
        {
            return false;
        }

        moduleRunDelegate(module);
        return true;
    }
}