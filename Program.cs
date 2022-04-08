using System.Runtime.CompilerServices;
using log4net;
using log4net.Config;
using TAB2;

const string TokenEnvironmentName = "TAB2_TOKEN";
const string LogConfigFile = "log4net_cfg.xml";

XmlConfigurator.Configure(new FileInfo(LogConfigFile));

ILog log = LogManager.GetLogger("Entry");

string? token = Environment.GetEnvironmentVariable(TokenEnvironmentName, EnvironmentVariableTarget.Machine);

if (token == null)
{
    log.Fatal($"Could not find bot token! Please set {TokenEnvironmentName} environment variable to your bot token!");
    return;
}

try
{
    log.Info($"Starting bot with token {token}");
    
    using var main = new BotMain();
    Task runTask = main.Run(token);
    TaskAwaiter awaiter = runTask.GetAwaiter();
    awaiter.GetResult();
}
catch (Exception e)
{
    log.Fatal("Crash!!", e);
}