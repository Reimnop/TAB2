﻿using System.Text;
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
        client.Ready += () => moduleManager.RunOnAllModulesAsync(module => Task.Run(() => module.BaseModule.OnReady()));
        client.MessageReceived += message => moduleManager.RunOnAllModulesAsync(module => Task.Run(() => module.BaseModule.OnMessageReceived(message)));
        
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
            CommandSource source = new CommandSource((SocketTextChannel) message.Channel);
            
            if (!await moduleManager.TryRunOnModuleAsync(
                    id,
                    module => RunCommand(module.CommandDispatcher, subCommand, (SocketTextChannel) message.Channel, source)
                    )
                )
            {
                await message.Channel.SendMessageAsync($"Module with id '{id}' does not exist!");
            }
        });
    }

    private async Task RunCommand(CommandDispatcher<CommandSource> dispatcher, string command, SocketTextChannel channel, CommandSource source)
    {
        try
        {
            dispatcher.Execute(command, source);
        }
        catch (CommandSyntaxException e)
        {
            await channel.SendMessageAsync(e.Message);
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