using System.Text;
using Brigadier.NET;
using Brigadier.NET.Exceptions;
using Discord;
using Discord.WebSocket;
using log4net;
using TAB2.Api.Command;
using TAB2.Module;

namespace TAB2;

public class BotMain : IDisposable
{
    private readonly ILog log;
    
    private readonly DiscordSocketClient client;
    private readonly ModuleManager moduleManager;

    public BotMain()
    {
        log = LogManager.GetLogger("Discord");
        
        DiscordSocketConfig config = new DiscordSocketConfig();
        config.DefaultRetryMode = RetryMode.AlwaysRetry;

        client = new DiscordSocketClient(config);

        moduleManager = new ModuleManager();
    }

    public async Task Run(string token)
    {
        moduleManager.LoadModules("Modules");
        
        // Module initialization steps
        await moduleManager.RunOnAllModulesAsync(module => Task.Run(() => module.BaseModule.OnCommandRegister(module.CommandDispatcher)));

        // Module events
        // Everything is a one-liner
        client.Ready += () => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnReady().Wait());
        client.ChannelCreated += channel => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnChannelCreated(channel).Wait());
        client.ChannelDestroyed += channel => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnChannelDestroyed(channel).Wait());
        client.ChannelUpdated += (oldChannel, newChannel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnChannelUpdated(oldChannel, newChannel).Wait());
        client.GuildAvailable += guild => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildAvailable(guild).Wait());
        client.GuildUnavailable += guild => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildUnavailable(guild).Wait());
        client.GuildUpdated += (oldGuild, newGuild) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildUpdated(oldGuild, newGuild).Wait());
        client.GuildMembersDownloaded += guild => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildMembersDownloaded(guild).Wait());
        client.JoinedGuild += guild => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnJoinedGuild(guild).Wait());
        client.LeftGuild += guild => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnLeftGuild(guild).Wait());
        client.MessageDeleted += (message, channel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnMessageDeleted(message, channel).Wait());
        client.MessagesBulkDeleted += (messages, channel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnMessagesBulkDeleted(messages, channel).Wait());
        client.MessageReceived += message => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnMessageReceived(message).Wait());
        client.MessageUpdated += (oldMessage, newMessage, channel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnMessageUpdated(oldMessage, newMessage, channel).Wait());
        client.ReactionAdded += (message, channel, reaction) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnReactionAdded(message, channel, reaction).Wait());
        client.ReactionRemoved += (message, channel, reaction) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnReactionRemoved(message, channel, reaction).Wait());
        client.ReactionsCleared += (message, channel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnReactionsCleared(message, channel).Wait());
        client.ReactionsRemovedForEmote += (message, channel, emote) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnReactionsRemovedForEmote(message, channel, emote).Wait());
        client.UserBanned += (user, guild) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserBanned(user, guild).Wait());
        client.UserJoined += user => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserJoined(user).Wait());
        client.UserLeft += (guild, user) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserLeft(guild, user).Wait());
        client.UserUnbanned += (user, guild) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserUnbanned(user, guild).Wait());
        client.UserUpdated += (oldUser, newUser) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserUpdated(oldUser, newUser).Wait());
        client.GuildMemberUpdated += (oldMember, newMember) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildMemberUpdated(oldMember, newMember).Wait());
        client.UserVoiceStateUpdated += (user, oldState, newState) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserVoiceStateUpdated(user, oldState, newState).Wait());
        client.VoiceServerUpdated += server => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnVoiceServerUpdated(server).Wait());
        client.RoleCreated += role => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRoleCreated(role).Wait());
        client.RoleDeleted += role => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRoleDeleted(role).Wait());
        client.RoleUpdated += (oldRole, newRole) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRoleUpdated(oldRole, newRole).Wait());
        client.GuildJoinRequestDeleted += (user, guild) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildJoinRequestDeleted(user, guild).Wait());
        client.GuildScheduledEventCreated += guildEvent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventCreated(guildEvent).Wait());
        client.GuildScheduledEventUpdated += (oldEvent, newEvent) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventUpdated(oldEvent, newEvent).Wait());
        client.GuildScheduledEventCancelled += guildEvent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventCancelled(guildEvent).Wait());
        client.GuildScheduledEventCompleted += guildEvent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventCompleted(guildEvent).Wait());
        client.GuildScheduledEventStarted += guildEvent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventStarted(guildEvent).Wait());
        client.GuildScheduledEventUserAdd += (user, guildEvent) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventUserAdd(user, guildEvent).Wait());
        client.GuildScheduledEventUserRemove += (user, guildEvent) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventUserRemove(user, guildEvent).Wait());
        client.IntegrationCreated += integration => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnIntegrationCreated(integration).Wait());
        client.IntegrationUpdated += integration => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnIntegrationUpdated(integration).Wait());
        client.IntegrationDeleted += (guild, guildId, integrationId) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnIntegrationDeleted(guild, guildId, integrationId).Wait());
        client.CurrentUserUpdated += (oldUser, newUser) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnCurrentUserUpdated(oldUser, newUser).Wait());
        client.UserIsTyping += (user, channel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserIsTyping(user, channel).Wait());
        client.RecipientAdded += user => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRecipientAdded(user).Wait());
        client.RecipientRemoved += user => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRecipientRemoved(user).Wait());
        client.PresenceUpdated += (user, oldPresence, newPresence) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnPresenceUpdated(user, oldPresence, newPresence).Wait());
        client.InviteCreated += invite => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnInviteCreated(invite).Wait());
        client.InviteDeleted += (channel, invite) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnInviteDeleted(channel, invite).Wait());
        client.InteractionCreated += interaction => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnInteractionCreated(interaction).Wait());
        client.ButtonExecuted += messageComponent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnButtonExecuted(messageComponent).Wait());
        client.SelectMenuExecuted += messageComponent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnSelectMenuExecuted(messageComponent).Wait());
        client.SlashCommandExecuted += slashCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnSlashCommandExecuted(slashCommand).Wait());
        client.UserCommandExecuted += userCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserCommandExecuted(userCommand).Wait());
        client.MessageCommandExecuted += messageCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnMessageCommandExecuted(messageCommand).Wait());
        client.AutocompleteExecuted += autocomplete => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnAutocompleteExecuted(autocomplete).Wait());
        client.ModalSubmitted += modal => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnModalSubmitted(modal).Wait());
        client.ApplicationCommandCreated += applicationCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnApplicationCommandCreated(applicationCommand).Wait());
        client.ApplicationCommandUpdated += applicationCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnApplicationCommandUpdated(applicationCommand).Wait());
        client.ApplicationCommandDeleted += applicationCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnApplicationCommandDeleted(applicationCommand).Wait());
        client.ThreadCreated += thread => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnThreadCreated(thread).Wait());
        client.ThreadUpdated += (oldThread, newThread) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnThreadUpdated(oldThread, newThread).Wait());
        client.ThreadDeleted += thread => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnThreadDeleted(thread).Wait());
        client.ThreadMemberJoined += threadUser => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnThreadMemberJoined(threadUser).Wait());
        client.ThreadMemberLeft += threadUser => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnThreadMemberLeft(threadUser).Wait());
        client.StageStarted += stage => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnStageStarted(stage).Wait());
        client.StageEnded += stage => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnStageEnded(stage).Wait());
        client.StageUpdated += (oldStage, newStage) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnStageUpdated(oldStage, newStage).Wait());
        client.RequestToSpeak += (channel, user) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRequestToSpeak(channel, user).Wait());
        client.SpeakerAdded += (channel, user) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnSpeakerAdded(channel, user).Wait());
        client.SpeakerRemoved += (channel, user) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnSpeakerRemoved(channel, user).Wait());
        client.GuildStickerCreated += customSicker => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildStickerCreated(customSicker).Wait());
        client.GuildStickerUpdated += (oldSticker, newSticker) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildStickerUpdated(oldSticker, newSticker).Wait());
        client.GuildStickerDeleted += customSicker => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildStickerDeleted(customSicker).Wait());
        client.Log += log => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnLog(log).Wait());
        client.LoggedIn += () => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnLoggedIn().Wait());
        client.LoggedOut += () => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnLoggedOut().Wait());
        
        client.Log += ClientOnLog;
        client.MessageReceived += ClientOnMessageReceived;

        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();

        await Task.Delay(Timeout.Infinite);
    }

    private Task ClientOnLog(LogMessage msg)
    {
        switch (msg.Severity)
        {
            case LogSeverity.Debug:
                log.Debug(msg.Message, msg.Exception);
                break;
            case LogSeverity.Info:
                log.Info(msg.Message, msg.Exception);
                break;
            case LogSeverity.Warning:
                log.Warn(msg.Message, msg.Exception);
                break;
            case LogSeverity.Error:
                log.Error(msg.Message, msg.Exception);
                break;
            case LogSeverity.Critical:
                log.Fatal(msg.Message, msg.Exception);
                break;
            case LogSeverity.Verbose:
                log.Debug(msg.Message, msg.Exception);
                break;
        }
        
        return Task.CompletedTask;
    }
    
    private Task ClientOnMessageReceived(SocketMessage message)
    {
        if (message.Author.IsBot)
        {
            return Task.CompletedTask;
        }

        if (message.Channel is not SocketGuildChannel)
        {
            return Task.CompletedTask;
        }

        if (!message.Content.StartsWith('!'))
        {
            return Task.CompletedTask;
        }

        string command = message.Content.Substring(1);
        SplitCommand(command, out string id, out string subCommand);
        
        return Task.Run(async () =>
        {
            CommandSource source = new CommandSource(message.Author, (SocketTextChannel) message.Channel);
            
            if (!moduleManager.TryRunOnModule(id, module => RunCommand(module.CommandDispatcher, source, subCommand).Wait()))
            {
                await message.Channel.SendMessageAsync($"Module with id '{id}' does not exist!");
            }
        });
    }

    private async Task RunCommand(CommandDispatcher<CommandSource> dispatcher, CommandSource source, string command)
    {
        try
        {
            dispatcher.Execute(command, source);
        }
        catch (CommandSyntaxException e)
        {
            await source.Channel.SendMessageAsync(e.Message);
        }
    }

    private void SplitCommand(string command, out string id, out string subCommand)
    {
        StringBuilder idBuilder = new StringBuilder();
        StringBuilder subCommandBuilder = new StringBuilder();

        int i = 0;
        
        // Get id
        for (; i < command.Length; i++)
        {
            if (command[i] == ' ')
            {
                break;
            }
            idBuilder.Append(command[i]);
        }
        
        // Skip spaces
        for (; i < command.Length; i++)
        {
            if (command[i] != ' ')
            {
                break;
            }
        }
        
        // Get the rest
        for (; i < command.Length; i++)
        {
            subCommandBuilder.Append(command[i]);
        }

        id = idBuilder.ToString();
        subCommand = subCommandBuilder.ToString();
    }

    public void Dispose()
    {
        client.Dispose();
    }
}