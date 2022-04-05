namespace TAB2.Configuration.Types;

public class IntegerValue : ConfigValue
{
    public override string TypeName => "Integer";

    private int value;
    
    public override bool TryParse(string value)
    {
        return int.TryParse(value, out this.value);
    }

    public override void SetValue(object value)
    {
        this.value = (int)value;
    }

    public override object GetValue()
    {
        return value;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}