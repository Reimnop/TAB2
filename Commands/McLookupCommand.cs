using System.Net;
using Discord;
using Discord.WebSocket;

namespace TAB2.Commands;

public class McLookupCommand : Command
{
    public override bool ShouldAddToGuild(SocketGuild guild)
    {
        return true;
    }

    public override SlashCommandBuilder GetSlashCommand()
    {
        var commandBuilder = new SlashCommandBuilder()
            .WithName("mclookup")
            .WithDescription("Looks up a Minecraft server.")
            .AddOption(new SlashCommandOptionBuilder()
                .WithName("ip")
                .WithDescription("IP of the server.")
                .WithRequired(true)
                .WithType(ApplicationCommandOptionType.String));
        return commandBuilder;
    }

    public override async Task ExecuteCommand(SocketSlashCommand command)
    {
        await command.DeferAsync();
        
        string ip = (string) command.Data.Options.First().Value;

        using var webClient = new WebClient();
        var stream = webClient.OpenRead($"https://api.loohpjames.com/serverbanner.png?ip={Uri.EscapeDataString(ip)}");

        await command.FollowupWithFileAsync(stream, "serverbanner.png");
    }

    public override void Dispose()
    {
    }
}