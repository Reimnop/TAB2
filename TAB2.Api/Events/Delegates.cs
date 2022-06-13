using Discord.WebSocket;

namespace TAB2.Api.Events;

public delegate void OnReadyDelegate();
public delegate void OnMessageReceivedDelegate(SocketMessage message);