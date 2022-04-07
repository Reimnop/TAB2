using Discord;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using TAB2.Configuration;

namespace TAB2.Commands;

public class ChatCommand : Command
{
    private readonly ConfigService configService;

    public ChatCommand(ConfigService configService)
    {
        this.configService = configService;
    }
    
    public override bool ShouldAddToGuild(SocketGuild guild)
    {
        return true;
    }

    public override SlashCommandBuilder GetSlashCommand()
    {
        var commandBuilder = new SlashCommandBuilder()
            .WithName("chat")
            .WithDescription("Chat with the bot! (inspired by Suzuhime#9658)")
            .AddOption(new SlashCommandOptionBuilder()
                .WithName("message")
                .WithType(ApplicationCommandOptionType.String)
                .WithDescription("Your chat message."));
        return commandBuilder;
    }

    public override async Task ExecuteCommand(SocketSlashCommand command)
    {
        await command.DeferAsync();

        string question = (string) command.Data.Options.First().Value;

        var message = new HttpRequestMessage(HttpMethod.Get, $"https://apiv2.spapi.ga/misc/clever?text={Uri.EscapeDataString(question)}");
        using var client = new HttpClient();
        var response = await client.SendAsync(message);
        var responseStr = await response.Content.ReadAsStringAsync();

        var json = JObject.Parse(responseStr);
        string answer = json["response"].ToString();

        var embedBuilder = new EmbedBuilder()
            .WithColor(ConfigHelper.GetEmbedColor(configService))
            .WithTitle(question)
            .WithDescription(answer);

        await command.FollowupAsync(embed: embedBuilder.Build());
    }

    public override void Dispose()
    {
    }
}