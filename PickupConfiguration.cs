global using Terraria.ModLoader;
global using Terraria;
global using Microsoft.Xna.Framework;
using Terraria.ID;
using System.Numerics;
using System;

namespace PickupConfiguration;

public class PickupConfiguration : Mod
{
    public override void Load() => On_PopupText.NewText_PopupTextContext_Item_int_bool_bool += HijackItemPopup;

    private int HijackItemPopup(On_PopupText.orig_NewText_PopupTextContext_Item_int_bool_bool orig, PopupTextContext context, Item newItem, int stack, bool noStack, bool longText)
    {
        PickupConfig.ConfigType type = ModContent.GetInstance<PickupConfig>().Type;

        if (type == PickupConfig.ConfigType.Chat)
        {
            Main.NewText($"{newItem.stack}x [i:{newItem.type}] [c/{GetRarityColor(newItem.rare).Hex3()}:{newItem.Name}");
            return -1;
        }
        else if (type == PickupConfig.ConfigType.Screen)
        {
            var text = new PopupText();
            text.active = true;
            text.color = GetRarityColor(newItem.rare);
            text.rarity = newItem.rare;
            text.expert = newItem.expert;
            text.master = newItem.master;
            text.name = newItem.AffixName();
            text.stack = stack;
            text.velocity.Y = -7f;
            text.lifeTime = 60;
            text.context = context;
            text.alpha = 1;

            long coinValue = 0L;
            if (newItem.type == ItemID.CopperCoin)
                coinValue += stack;
            else if (newItem.type == ItemID.SilverCoin)
            {
                coinValue += 100 * stack;
            }
            else if (newItem.type == ItemID.GoldCoin)
            {
                coinValue += 10000 * stack;
            }
            else if (newItem.type == ItemID.PlatinumCoin)
            {
                coinValue += 1000000 * stack;
            }

            if (coinValue > 0)
            {
                text.coinValue = coinValue;
                text.coinText = true;
                UpdateTextCoinValues(text);
            }

            ScreenPickupUI.AddPickupText(text);
            return -1;
        }

        return orig(context, newItem, stack, noStack, longText);
    }

    public static void UpdateTextCoinValues(PopupText text)
    {
        if (text.coinValue >= 1000000) // Platinum
        {
            if (text.lifeTime < 300)
                text.lifeTime = 300;

            text.color = new Color(220, 220, 198);
        }
        else if (text.coinValue >= 10000) // Gold
        {
            if (text.lifeTime < 240)
                text.lifeTime = 240;

            text.color = new Color(224, 201, 92);
        }
        else if (text.coinValue >= 100) // Silver
        {
            if (text.lifeTime < 180)
                text.lifeTime = 180;

            text.color = new Color(181, 192, 193);
        }
        else if (text.coinValue >= 1) // Copper
        {
            if (text.lifeTime < 120)
                text.lifeTime = 120;

            text.color = new Color(246, 138, 96);
        }
    }

    public static Color GetRarityColor(int rare)
    {
        Color color = Color.White;

        if (rare == 1)
            color = new Color(150, 150, 255);
        else if (rare == 2)
            color = new Color(150, 255, 150);
        else if (rare == 3)
            color = new Color(255, 200, 150);
        else if (rare == 4)
            color = new Color(255, 150, 150);
        else if (rare == 5)
            color = new Color(255, 150, 255);
        else if (rare == -11)
            color = new Color(255, 175, 0);
        else if (rare == -1)
            color = new Color(130, 130, 130);
        else if (rare == 6)
            color = new Color(210, 160, 255);
        else if (rare == 7)
            color = new Color(150, 255, 10);
        else if (rare == 8)
            color = new Color(255, 255, 10);
        else if (rare == 9)
            color = new Color(5, 200, 255);
        else if (rare == 10)
            color = new Color(255, 40, 100);
        else if (rare == 11)
            color = new Color(180, 40, 255);
        else if (rare >= 12)
            color = RarityLoader.GetRarity(rare).RarityColor;

        return color;
    }
}
