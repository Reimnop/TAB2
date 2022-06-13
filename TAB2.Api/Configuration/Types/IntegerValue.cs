namespace TAB2.Api.Configuration.Types;

public class IntegerValue : ConfigValue<int>
{
    public override string TypeName => "Integer";

    private int value;
    
    public override bool TryParse(string value)
    {
        return int.TryParse(value, out this.value);
    }
    
    public override string EncodeToString()
    {
        return value.ToString();
    }

    public override void SetValue(int value)
    {
        this.value = value;
    }

    public override int GetValue()
    {
        return value;
    }
}