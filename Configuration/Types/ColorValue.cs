using Discord;

namespace TAB2.Configuration.Types;

public class ColorValue : ConfigValue
{
    public override string TypeName { get; }

    private Color color;
    
    public override bool TryParse(string value)
    {
        try
        {
            if (value[0] == '#')
            {
                value = value.Substring(1, value.Length - 1);
            }

            value = value.ToLower();

            int r1 = value[0] > '9' ? value[0] - 'a' + 10 : value[0] - '0';
            int r2 = value[1] > '9' ? value[1] - 'a' + 10 : value[1] - '0';
            int g1 = value[2] > '9' ? value[2] - 'a' + 10 : value[2] - '0';
            int g2 = value[3] > '9' ? value[3] - 'a' + 10 : value[3] - '0';
            int b1 = value[4] > '9' ? value[4] - 'a' + 10 : value[4] - '0';
            int b2 = value[5] > '9' ? value[5] - 'a' + 10 : value[5] - '0';

            int r = r1 * 16 + r2;
            int g = g1 * 16 + g2;
            int b = b1 * 16 + b2;

            color = new Color(r, g, b);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public override void SetValue(object value)
    {
        color = (Color) value;
    }

    public override object GetValue()
    {
        return color;
    }

    public override string ToString()
    {
        return color.ToString();
    }
}