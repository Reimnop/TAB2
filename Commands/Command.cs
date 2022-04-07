using Discord;
using Discord.WebSocket;

namespace TAB2.Commands;

public abstract class Command : IDisposable
{
    public abstract bool ShouldAddToGuild(SocketGuild guild);
    public abstract SlashCommandBuilder GetSlashCommand();
    public abstract Task ExecuteCommand(SocketSlashCommand command);
    public abstract void Dispose();
}