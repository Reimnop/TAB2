using Brigadier.NET;
using TAB2.Api.Command;
using TAB2.Api.Module;

namespace TAB2.Module;

public class Module
{
    public BaseModule BaseModule { get; set; }
    public ModuleEntryAttribute Attribute { get; set; }
    public CommandDispatcher<CommandSource> CommandDispatcher { get; set; }

    public Module(BaseModule baseModule, ModuleEntryAttribute attribute)
    {
        BaseModule = baseModule;
        Attribute = attribute;

        CommandDispatcher = new CommandDispatcher<CommandSource>();
    }
}