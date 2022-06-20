using Discord.WebSocket;

namespace TAB2.Api.Command;

public class CommandSource
{
    public SocketUser User { get; }
    public SocketTextChannel Channel { get; }

    public CommandSource(SocketUser user, SocketTextChannel channel)
    {
        User = user;
        Channel = channel;
    }
}