namespace TAB2.Configuration.Types;

public abstract class ConfigValue
{
    public abstract string TypeName { get; }
    
    public abstract bool TryParse(string value);

    public abstract void SetValue(object value);
    public abstract object GetValue();

    public abstract override string ToString();
}