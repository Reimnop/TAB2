using System.Diagnostics.CodeAnalysis;
using Discord;
using Newtonsoft.Json.Linq;
using TAB2.Configuration.Types;

namespace TAB2.Configuration;

public class ConfigValueInfo
{
    public string Name { get; set; }
    public ConfigValue Value { get; set; }
}

public class ConfigService
{
    private const string ConfigFileName = "config.json";
    
    private Dictionary<string, int> configValueIndices = new();
    private List<ConfigValueInfo> configValueInfos = new();

    public ConfigService()
    {
        RegisterConfigValues();
        ReadConfigValues();
    }

    private void RegisterConfigValues()
    {
        RegisterConfigValue<ColorValue>("embedColor", Color.Green);
    }

    public IEnumerable<ConfigValueInfo> GetConfigValues()
    {
        return configValueInfos;
    }

    public bool TryGetConfigValue(string key, [MaybeNullWhen(false)] out ConfigValue value)
    {
        if (configValueIndices.TryGetValue(key, out var i))
        {
            value = configValueInfos[i].Value;
            return true;
        }

        value = null;
        return false;
    }

    public ConfigValue GetConfigValue(string key)
    {
        if (TryGetConfigValue(key, out var value))
        {
            return value;
        }

        throw new KeyNotFoundException($"Could not find the config key named \"{key}\"!");
    }

    public void ReadConfigValues()
    {
        // If config doesn't exist, write and do nothing else
        if (!File.Exists(ConfigFileName))
        {
            WriteConfigValues();
            return;
        }

        string jsonString = File.ReadAllText(ConfigFileName);
        JObject json = JObject.Parse(jsonString);
        foreach (var kvp in json)
        {
            if (TryGetConfigValue(kvp.Key, out var value))
            {
                value.TryParse(kvp.Value.ToString());
            }
        }
    }

    public void WriteConfigValues()
    {
        JObject json = new JObject();
        foreach (var info in configValueInfos)
        {
            json[info.Name] = info.Value.ToString();
        }
        File.WriteAllText(ConfigFileName, json.ToString());
    }

    private void RegisterConfigValue<T>(string name, object defaultValue) where T : ConfigValue, new()
    {
        T value = new T();
        value.SetValue(defaultValue);
        configValueIndices.Add(name, configValueInfos.Count);
        configValueInfos.Add(new ConfigValueInfo
        {
            Name = name,
            Value = value
        });
    }
}