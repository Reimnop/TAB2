using Discord;
using Discord.WebSocket;
using TAB2.Commands;

namespace TAB2;

public class Tab2Main : IDisposable
{
    private readonly DiscordSocketClient client;
    private readonly CommandList commands;

    public Tab2Main()
    {
        DiscordSocketConfig config = new DiscordSocketConfig();
        client = new DiscordSocketClient(config);
        commands = new CommandList(client);
    }

    public async Task Run(string token)
    {
        client.Log += ClientOnLog;
        client.SlashCommandExecuted += ClientOnSlashCommandExecuted; 
        
        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();
        
        await Task.Delay(Timeout.Infinite);
    }

    private async Task ClientOnSlashCommandExecuted(SocketSlashCommand command)
    {
        await commands.ExecuteCommand(command);
    }

    private Task ClientOnLog(LogMessage arg)
    {
        Console.WriteLine(arg.ToString());
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        client.Dispose();
    }
}