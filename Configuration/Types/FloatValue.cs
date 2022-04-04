namespace TAB2.Configuration.Types;

public class FloatValue : ConfigValue
{
    public override string TypeName => "Float";

    private float value;
    
    public override bool TryParse(string value)
    {
        return float.TryParse(value, out this.value);
    }

    public override void SetValue(object value)
    {
        this.value = (float) value;
    }

    public override object GetValue()
    {
        return value;
    }
}