using Discord;
using Discord.WebSocket;
using TAB2.Configuration;

namespace TAB2.Commands;

public class PingCommand : Command
{
    private readonly DiscordSocketClient client;
    private readonly ConfigService configService;

    public PingCommand(DiscordSocketClient client, ConfigService configService)
    {
        this.client = client;
        this.configService = configService;
    }
    
    public override SlashCommandBuilder GetSlashCommand()
    {
        var commandBuilder = new SlashCommandBuilder()
            .WithName("ping")
            .WithDescription("Ping.");
        return commandBuilder;
    }

    public override async Task ExecuteCommand(SocketSlashCommand command)
    {
        int latency = client.Latency;

        var embedBuilder = new EmbedBuilder()
            .WithTitle("Pong!")
            .WithDescription($"Latency: {latency}ms")
            .WithColor(ConfigHelper.GetEmbedColor(configService));

        await command.RespondAsync(embed: embedBuilder.Build(), ephemeral: true);
    }

    public override void Dispose()
    {
    }
}