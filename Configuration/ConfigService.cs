using System.Diagnostics.CodeAnalysis;
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
        RegisterConfigValue<IntegerValue>("testValue", 0);
        RegisterConfigValue<FloatValue>("testValueF", 2.0f);
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
            configValueInfos[configValueIndices[kvp.Key]].Value.TryParse(kvp.Value.ToString());
        }
    }

    public void WriteConfigValues()
    {
        JObject json = new JObject();
        foreach (var info in configValueInfos)
        {
            json[info.Name] = info.Value.GetValue().ToString();
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