using Brigadier.NET;
using Discord.WebSocket;
using TAB2.Api.Command;

namespace TAB2.Api.Module;

public abstract class BaseModule
{
    public abstract void Initialize();

    public virtual void OnReady() 
    {
    }

    public virtual void OnCommandRegister(CommandDispatcher<CommandSource> dispatcher)
    {
    }

    public virtual void OnMessageReceived(SocketMessage message)
    {
    }
}