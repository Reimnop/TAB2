using Discord;
using Discord.WebSocket;
using TAB2.Configuration;

namespace TAB2.Commands;

public class StatsCommand : Command
{
    private readonly ConfigService configService;

    public StatsCommand(ConfigService configService)
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
            .WithName("stats")
            .WithDescription("Get server stats.");
        return commandBuilder;
    }

    public override async Task ExecuteCommand(SocketSlashCommand command)
    {
        if (command.Channel is not SocketGuildChannel channel)
        {
            await command.RespondAsync("This command must be run in a guild!");
            return;
        }

        var guild = channel.Guild;

        int total = guild.MemberCount;
        int online = 0, idle = 0, dnd = 0, offline = 0;

        foreach (var user in guild.Users)
        {
            online += user.Status == UserStatus.Online ? 1 : 0;
            idle += user.Status == UserStatus.Idle || user.Status == UserStatus.AFK ? 1 : 0;
            dnd += user.Status == UserStatus.DoNotDisturb ? 1 : 0;
            offline += user.Status == UserStatus.Offline || user.Status == UserStatus.Invisible ? 1 : 0;
        }

        var embedBuilder = new EmbedBuilder()
            .WithColor(ConfigHelper.GetEmbedColor(configService))
            .WithTitle(guild.Name)
            .WithThumbnailUrl(guild.IconUrl)
            .WithCurrentTimestamp()
            .AddField(new EmbedFieldBuilder()
                .WithName("Members")
                .WithValue($"Total: {total}\nOnline: {online}\nIdle: {idle}\nDo Not Disturb: {dnd}\nOffline: {offline}"))
            .AddField(new EmbedFieldBuilder()
                .WithName("Channels")
                .WithValue($"Total: {guild.Channels.Count}"));

        await command.RespondAsync(embed: embedBuilder.Build());
    }

    public override void Dispose()
    {
    }
}