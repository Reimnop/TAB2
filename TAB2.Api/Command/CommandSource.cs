using Discord.WebSocket;

namespace TAB2.Api.Command;

public class CommandSource
{
    public SocketTextChannel Channel { get; }

    public CommandSource(SocketTextChannel channel)
    {
        Channel = channel;
    }
}