using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace PickupConfiguration;

internal class PickupConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    public enum ConfigType : byte
    {
        Screen,
        Chat,
        Off
    }

    [DefaultValue(ConfigType.Screen)]
    public ConfigType Type { get; set; }

    [DefaultValue(8)]
    public int MaxPickups { get; set; }

    [DefaultValue("1, 1")]
    public Vector2 PositionOnScreen { get; set; }

    [DefaultValue(1f)]
    [Range(0.5f, 3f)]
    public float Scale { get; set; }

    [DefaultValue(2f)]
    [Range(0.5f, 5f)]
    public float TextLifeTimeMultiplier { get; set; }
}
