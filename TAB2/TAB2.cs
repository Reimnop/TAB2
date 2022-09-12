using System.Text;
using Brigadier.NET;
using Brigadier.NET.Exceptions;
using Discord;
using Discord.WebSocket;
using log4net;
using TAB2.Api;
using TAB2.Api.Command;
using TAB2.Module;

namespace TAB2;

public class TAB2 : IDisposable, IBotInstance
{
    public DiscordSocketClient Client => client;

    private readonly ILog log;
    
    private readonly DiscordSocketClient client;
    private readonly ModuleManager moduleManager;

    public TAB2()
    {
        log = LogManager.GetLogger("Discord");
        
        DiscordSocketConfig config = new DiscordSocketConfig();
        config.DefaultRetryMode = RetryMode.AlwaysRetry;

        client = new DiscordSocketClient(config);

        moduleManager = new ModuleManager();
    }

    public async Task Run(string token)
    {
        moduleManager.LoadModules("Modules", this);
        
        // Module initialization steps
        await moduleManager.RunOnAllModulesAsync(module => Task.Run(() => module.BaseModule.OnCommandRegister(module.CommandDispatcher)));

        #region ModuleEvents
        // Module events
        // Everything is a one-liner
        client.Ready += () => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnReady());
        client.ChannelCreated += channel => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnChannelCreated(channel));
        client.ChannelDestroyed += channel => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnChannelDestroyed(channel));
        client.ChannelUpdated += (oldChannel, newChannel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnChannelUpdated(oldChannel, newChannel));
        client.GuildAvailable += guild => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildAvailable(guild));
        client.GuildUnavailable += guild => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildUnavailable(guild));
        client.GuildUpdated += (oldGuild, newGuild) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildUpdated(oldGuild, newGuild));
        client.GuildMembersDownloaded += guild => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildMembersDownloaded(guild));
        client.JoinedGuild += guild => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnJoinedGuild(guild));
        client.LeftGuild += guild => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnLeftGuild(guild));
        client.MessageDeleted += (message, channel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnMessageDeleted(message, channel));
        client.MessagesBulkDeleted += (messages, channel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnMessagesBulkDeleted(messages, channel));
        client.MessageReceived += message => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnMessageReceived(message));
        client.MessageUpdated += (oldMessage, newMessage, channel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnMessageUpdated(oldMessage, newMessage, channel));
        client.ReactionAdded += (message, channel, reaction) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnReactionAdded(message, channel, reaction));
        client.ReactionRemoved += (message, channel, reaction) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnReactionRemoved(message, channel, reaction));
        client.ReactionsCleared += (message, channel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnReactionsCleared(message, channel));
        client.ReactionsRemovedForEmote += (message, channel, emote) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnReactionsRemovedForEmote(message, channel, emote));
        client.UserBanned += (user, guild) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserBanned(user, guild));
        client.UserJoined += user => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserJoined(user));
        client.UserLeft += (guild, user) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserLeft(guild, user));
        client.UserUnbanned += (user, guild) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserUnbanned(user, guild));
        client.UserUpdated += (oldUser, newUser) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserUpdated(oldUser, newUser));
        client.GuildMemberUpdated += (oldMember, newMember) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildMemberUpdated(oldMember, newMember));
        client.UserVoiceStateUpdated += (user, oldState, newState) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserVoiceStateUpdated(user, oldState, newState));
        client.VoiceServerUpdated += server => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnVoiceServerUpdated(server));
        client.RoleCreated += role => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRoleCreated(role));
        client.RoleDeleted += role => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRoleDeleted(role));
        client.RoleUpdated += (oldRole, newRole) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRoleUpdated(oldRole, newRole));
        client.GuildJoinRequestDeleted += (user, guild) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildJoinRequestDeleted(user, guild));
        client.GuildScheduledEventCreated += guildEvent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventCreated(guildEvent));
        client.GuildScheduledEventUpdated += (oldEvent, newEvent) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventUpdated(oldEvent, newEvent));
        client.GuildScheduledEventCancelled += guildEvent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventCancelled(guildEvent));
        client.GuildScheduledEventCompleted += guildEvent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventCompleted(guildEvent));
        client.GuildScheduledEventStarted += guildEvent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventStarted(guildEvent));
        client.GuildScheduledEventUserAdd += (user, guildEvent) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventUserAdd(user, guildEvent));
        client.GuildScheduledEventUserRemove += (user, guildEvent) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventUserRemove(user, guildEvent));
        client.IntegrationCreated += integration => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnIntegrationCreated(integration));
        client.IntegrationUpdated += integration => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnIntegrationUpdated(integration));
        client.IntegrationDeleted += (guild, guildId, integrationId) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnIntegrationDeleted(guild, guildId, integrationId));
        client.CurrentUserUpdated += (oldUser, newUser) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnCurrentUserUpdated(oldUser, newUser));
        client.UserIsTyping += (user, channel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserIsTyping(user, channel));
        client.RecipientAdded += user => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRecipientAdded(user));
        client.RecipientRemoved += user => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRecipientRemoved(user));
        client.PresenceUpdated += (user, oldPresence, newPresence) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnPresenceUpdated(user, oldPresence, newPresence));
        client.InviteCreated += invite => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnInviteCreated(invite));
        client.InviteDeleted += (channel, invite) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnInviteDeleted(channel, invite));
        client.InteractionCreated += interaction => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnInteractionCreated(interaction));
        client.ButtonExecuted += messageComponent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnButtonExecuted(messageComponent));
        client.SelectMenuExecuted += messageComponent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnSelectMenuExecuted(messageComponent));
        client.SlashCommandExecuted += slashCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnSlashCommandExecuted(slashCommand));
        client.UserCommandExecuted += userCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserCommandExecuted(userCommand));
        client.MessageCommandExecuted += messageCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnMessageCommandExecuted(messageCommand));
        client.AutocompleteExecuted += autocomplete => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnAutocompleteExecuted(autocomplete));
        client.ModalSubmitted += modal => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnModalSubmitted(modal));
        client.ApplicationCommandCreated += applicationCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnApplicationCommandCreated(applicationCommand));
        client.ApplicationCommandUpdated += applicationCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnApplicationCommandUpdated(applicationCommand));
        client.ApplicationCommandDeleted += applicationCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnApplicationCommandDeleted(applicationCommand));
        client.ThreadCreated += thread => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnThreadCreated(thread));
        client.ThreadUpdated += (oldThread, newThread) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnThreadUpdated(oldThread, newThread));
        client.ThreadDeleted += thread => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnThreadDeleted(thread));
        client.ThreadMemberJoined += threadUser => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnThreadMemberJoined(threadUser));
        client.ThreadMemberLeft += threadUser => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnThreadMemberLeft(threadUser));
        client.StageStarted += stage => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnStageStarted(stage));
        client.StageEnded += stage => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnStageEnded(stage));
        client.StageUpdated += (oldStage, newStage) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnStageUpdated(oldStage, newStage));
        client.RequestToSpeak += (channel, user) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRequestToSpeak(channel, user));
        client.SpeakerAdded += (channel, user) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnSpeakerAdded(channel, user));
        client.SpeakerRemoved += (channel, user) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnSpeakerRemoved(channel, user));
        client.GuildStickerCreated += customSicker => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildStickerCreated(customSicker));
        client.GuildStickerUpdated += (oldSticker, newSticker) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildStickerUpdated(oldSticker, newSticker));
        client.GuildStickerDeleted += customSicker => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildStickerDeleted(customSicker));
        client.Log += log => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnLog(log));
        client.LoggedIn += () => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnLoggedIn());
        client.LoggedOut += () => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnLoggedOut());
        #endregion
        
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
            CommandSource source = new CommandSource(message);
            
            if (!await moduleManager.TryRunOnModuleAsync(id, module => RunCommand(module.CommandDispatcher, source, subCommand)))
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
            await source.Message.Channel.SendMessageAsync(e.Message);
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