using Discord;
using Discord.WebSocket;
using TAB2.Configuration;

namespace TAB2.Commands;

public class ConfigCommand: Command
{
    private readonly ConfigService configService;

    public ConfigCommand(ConfigService configService)
    {
        this.configService = configService;
    }


    public override SlashCommandBuilder GetSlashCommand()
    {
        var keyOptionBuilder = new SlashCommandOptionBuilder()
            .WithName("key")
            .WithDescription("The config key.")
            .WithType(ApplicationCommandOptionType.String)
            .WithRequired(false);
        foreach (var info in configService.GetConfigValues())
        {
            keyOptionBuilder.AddChoice(info.Name, info.Name);
        }
        
        SlashCommandBuilder commandBuilder = new SlashCommandBuilder()
            .WithName("config")
            .WithDescription("Gets a sets bot config.")
            .AddOption(new SlashCommandOptionBuilder()
                .WithName("action")
                .WithDescription("The action to be performed.")
                .WithRequired(true)
                .WithType(ApplicationCommandOptionType.Integer)
                .AddChoice("List", 0)
                .AddChoice("Get", 1)
                .AddChoice("Set", 2))
            .AddOption(keyOptionBuilder)
            .AddOption("value", ApplicationCommandOptionType.String, "The config value.", isRequired: false);
        return commandBuilder;
    }

    public override async Task ExecuteCommand(SocketSlashCommand command)
    {
        var options = command.Data.Options.ToList();
        long action = (long) options[0].Value;

        switch (action)
        {
            case 0: // list
            {
                var embedBuilder = new EmbedBuilder()
                    .WithTitle("Config list")
                    .WithColor(Color.Green)
                    .WithCurrentTimestamp();

                foreach (var info in configService.GetConfigValues())
                {
                    var fieldBuilder = new EmbedFieldBuilder()
                        .WithName(info.Name)
                        .WithValue($"Type: `{info.Value.TypeName}`\nValue: `{info.Value.GetValue()}`");
                    embedBuilder.AddField(fieldBuilder);
                }

                await command.RespondAsync(embed: embedBuilder.Build());
                
                break;
            }
            case 1: // get
            {
                if (options.Count < 2)
                {
                    await command.RespondAsync("Error: Key not specified!");
                    return;
                }
                
                string key = (string) options[1].Value;
                
                if (configService.TryGetConfigValue(key, out var value))
                {
                    var embedBuilder = new EmbedBuilder()
                        .WithTitle(key)
                        .WithDescription(value.GetValue().ToString())
                        .WithColor(Color.Green)
                        .WithCurrentTimestamp();
                
                    await command.RespondAsync(embed: embedBuilder.Build());
                }
                else
                {
                    await command.RespondAsync($"The config value \"{key}\" does not exist!", ephemeral: true);
                }
                
                break;
            }
            case 2: // set
            {
                if (options.Count < 2)
                {
                    await command.RespondAsync("Error: Key not specified!");
                    return;
                }

                if (options.Count < 3)
                {
                    await command.RespondAsync("Error: Value not specified!");
                    return;
                }
                
                string key = (string) options[1].Value;
                string valueStr = (string) options[2].Value;
                
                if (configService.TryGetConfigValue(key, out var value))
                {
                    object oldValue = value.GetValue();
                    
                    if (value.TryParse(valueStr))
                    {
                        configService.WriteConfigValues();
                        
                        var embedBuilder = new EmbedBuilder()
                            .WithTitle(key)
                            .WithDescription($"{oldValue} -> {value.GetValue().ToString()}")
                            .WithColor(Color.Green)
                            .WithCurrentTimestamp();
                
                        await command.RespondAsync(embed: embedBuilder.Build());
                    }
                    else
                    {
                        await command.RespondAsync($"Could not convert \"{valueStr}\" to type `{value.TypeName}`!", ephemeral: true);
                    }
                }
                else
                {
                    await command.RespondAsync($"The config value \"{key}\" does not exist!", ephemeral: true);
                }
                
                break;
            }
        }
    }

    public override void Dispose()
    {
    }
}