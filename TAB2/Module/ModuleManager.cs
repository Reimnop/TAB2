using System.Reflection;
using log4net;
using TAB2.Api.Events;
using TAB2.Utilities;

namespace TAB2.Module;

public delegate void ModuleRunDelegate(Module module);

public class ModuleManager
{
    private readonly ILog log;
    private readonly Dictionary<string, Module> loadedModules;
    private readonly DeferredRunner deferredRunner;

    public ModuleManager()
    {
        log = LogManager.GetLogger("ModuleManager");
        loadedModules = new Dictionary<string, Module>();
        deferredRunner = new DeferredRunner();
    }

    public void LoadModules(string directory)
    {
        FileInfo[] files = new DirectoryInfo(directory).GetFiles();
        IEnumerable<FileInfo> assemblyFiles = files
            .Where(x => x.Extension == ".dll");
        foreach (FileInfo assemblyFile in assemblyFiles)
        {
            LoadModule(assemblyFile.FullName);
        }
    }

    public void LoadModule(string path)
    {
        Assembly assembly = Assembly.LoadFile(path);
        ModuleLoader loader = new ModuleLoader(assembly);

        // Attribute should never be null if module isn't null
        ModuleInfo? info = loader.GetModuleInfo();
        if (info == null)
        {
            log.Warn($"Assembly '{assembly}' does not define a module entry!");
            return;
        }

        ModuleEventBus eventBus = new ModuleEventBus();
        Module module = new Module(info, eventBus);
        loadedModules.Add(info.Attribute.Id, module);
        
        deferredRunner.QueueFunction(() =>
        {
            log.Info($"Loading module '{info.Attribute.Name}' [{info.Attribute.Version}] (id: '{info.Attribute.Id}')");
            info.EntryPoint.Initialize(eventBus);
        });
    }

    public void InitializeModules()
    {
        deferredRunner.RunAll();
    }

    public Task RunOnAllModules(ModuleRunDelegate moduleRunDelegate)
    {
        foreach (Module module in loadedModules.Values)
        {
            moduleRunDelegate(module);
        }
        
        return Task.CompletedTask;
    }
}