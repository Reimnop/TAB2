using Discord.WebSocket;

namespace TAB2.Api.Events;

public class ModuleEventBus
{
    public event OnReadyDelegate OnReady;
    public event OnMessageReceivedDelegate OnMessageReceived;

    public void RaiseReadyEvent()
    {
        OnReady.Invoke();
    }

    public void RaiseMessageReceivedEvent(SocketMessage message)
    {
        OnMessageReceived.Invoke(message);
    }
}