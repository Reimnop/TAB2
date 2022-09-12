using System.Runtime.CompilerServices;
using log4net;
using log4net.Config;

const string LogConfigFile = "log4net_cfg.xml";

XmlConfigurator.Configure(new FileInfo(LogConfigFile));

ILog log = LogManager.GetLogger("Entry");

string? token = args.Length > 0 ? args[0] : null;

if (token == null)
{
    log.Fatal("No bot token!");
    return;
}

try
{
    log.Info("Starting TAB2 Loader");
    
    using var main = new TAB2.TAB2();
    Task runTask = main.Run(token);
    TaskAwaiter awaiter = runTask.GetAwaiter();
    awaiter.GetResult();
}
catch (Exception e)
{
    log.Fatal("Crash!!", e);
}