using Discord.WebSocket;

namespace TAB2.Api;

public interface IBotInstance
{
    DiscordSocketClient Client { get; }
    string ConfigPath { get; }
}