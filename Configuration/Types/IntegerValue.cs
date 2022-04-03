namespace TAB2.Configuration.Types;

public class IntegerValue : ConfigValue
{
    public override string TypeName => "Integer";

    private int value;
    
    public override bool TryParse(string value)
    {
        if (int.TryParse(value, out int v))
        {
            this.value = v;
            return true;
        }

        return false;
    }

    public override void SetValue(object value)
    {
        this.value = (int)value;
    }

    public override object GetValue()
    {
        return value;
    }
}