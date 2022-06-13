namespace TAB2.Api.Configuration.Types;

public class DoubleValue : ConfigValue<double>
{
    public override string TypeName => "Float";

    private double value;
    
    public override bool TryParse(string value)
    {
        return double.TryParse(value, out this.value);
    }
    
    public override string EncodeToString()
    {
        return value.ToString();
    }

    public override void SetValue(double value)
    {
        this.value = value;
    }

    public override double GetValue()
    {
        return value;
    }
}