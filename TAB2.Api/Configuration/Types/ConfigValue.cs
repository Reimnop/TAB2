namespace TAB2.Api.Configuration.Types;

public interface IConfigValue
{
    public string TypeName { get; }
    public bool TryParse(string value);
    public string EncodeToString();
}

public abstract class ConfigValue<T> : IConfigValue
{
    public abstract string TypeName { get; }
    
    public abstract bool TryParse(string value);
    public abstract string EncodeToString();

    public abstract void SetValue(T value);
    public abstract T GetValue();
}