using System.Reflection;
using System.Text;
using log4net;
using TAB2.Api.Module;

namespace TAB2.Module;

public delegate void ModuleRunDelegate(Module module);
public delegate Task ModuleTaskDelegate(Module module);

public class ModuleManager
{
    private readonly ILog log;
    private readonly Dictionary<string, int> moduleIndices;
    private readonly List<Module> loadedModules;

    public ModuleManager()
    {
        log = LogManager.GetLogger("ModuleManager");
        moduleIndices = new Dictionary<string, int>();
        loadedModules = new List<Module>();
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
            module.BaseModule.Initialize();
            
            moduleIndices.Add(module.Attribute.Id, loadedModules.Count);
            loadedModules.Add(module);
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
        if (!ModuleLoader.TryLoadModule(assembly, out BaseModule? entryPoint, out ModuleEntryAttribute? attribute))
        {
            return null;
        }
        
        return new Module(entryPoint, attribute);
    }

    public async void RunOnAllModules(ModuleRunDelegate moduleRunDelegate)
    {
        foreach (Module module in loadedModules)
        {
            moduleRunDelegate(module);
        }
    }
    
    public async Task RunOnAllModulesAsync(ModuleTaskDelegate moduleTaskDelegate)
    {
        List<Task> tasks = new List<Task>(loadedModules.Count);
        foreach (Module module in loadedModules)
        {
            tasks.Add(Task.Run(() => moduleTaskDelegate(module)));
        }
        
        await Task.WhenAll(tasks);
    }

    public bool TryRunOnModule(string id, ModuleRunDelegate moduleRunDelegate)
    {
        if (!moduleIndices.TryGetValue(id, out int index))
        {
            return false;
        }

        moduleRunDelegate(loadedModules[index]);
        return true;
    }
    
    public Task<bool> TryRunOnModuleAsync(string id, ModuleTaskDelegate moduleTaskDelegate)
    {
        if (!moduleIndices.TryGetValue(id, out int index))
        {
            return Task.FromResult(false);
        }

        moduleTaskDelegate(loadedModules[index]);
        return Task.FromResult(true);
    }
}