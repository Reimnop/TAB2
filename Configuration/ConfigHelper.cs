using Discord;

namespace TAB2.Configuration;

public static class ConfigHelper
{
    public static Color GetEmbedColor(ConfigService configService)
    {
        return (Color) configService.GetConfigValue("embedColor").GetValue();
    }
}