using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.UI.Chat;

namespace PickupConfiguration;

internal class ScreenPickupUIState : UIState
{
    private List<PopupText> _pickups = [];

    public void AddPickup(PopupText text)
    {
        if (!text.notActuallyAnItem)
        {
            for (int i = 0; i < _pickups.Count; ++i)
            {
                PopupText oldText = _pickups[i];

                if (oldText.coinText && text.coinText)
                {
                    text.coinValue += oldText.coinValue;
                    _pickups.RemoveAt(i);
                    i--;
                }
                else if (oldText.name == text.name)
                {
                    text.stack += oldText.stack;
                    _pickups.RemoveAt(i);
                    i--;
                }
            }
        }

        if (text.coinText)
            PickupConfiguration.UpdateTextCoinValues(text);

        text.lifeTime = (int)(text.lifeTime * ModContent.GetInstance<PickupConfig>().TextLifeTimeMultiplier);
        _pickups.Add(text);

        if (_pickups.Count > ModContent.GetInstance<PickupConfig>().MaxPickups)
            _pickups.RemoveAt(0);
    }

    public override void Update(GameTime gameTime)
    {
        for (int i = 0; i < _pickups.Count; i++)
        {
            PopupText pickup = _pickups[i];

            if (pickup.lifeTime-- < 0)
                pickup.alpha -= 0.015f;
        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        if (ModContent.GetInstance<PickupConfig>().Type != PickupConfig.ConfigType.Screen)
            return;

        float scale = ModContent.GetInstance<PickupConfig>().Scale;
        Vector2 screenPos = ModContent.GetInstance<PickupConfig>().PositionOnScreen;

        for (int i = 0; i < _pickups.Count; i++)
        {
            PopupText pickup = _pickups[i];

            if (pickup.expert)
                pickup.color = new Color((byte)Main.DiscoR, (byte)Main.DiscoG, (byte)Main.DiscoB, Main.mouseTextColor);
            else if (pickup.master)
                pickup.color = new Color(255, (byte)(Main.masterColor * 200f), 0, Main.mouseTextColor);

            string text = pickup.stack + "x " + pickup.name;

            if (pickup.coinText)
                text = PopupText.ValueToName(pickup.coinValue);

            Vector2 size = FontAssets.MouseText.Value.MeasureString(text) * scale;
            var pos = GetPosition(i, screenPos, scale, size);
            Color col = pickup.color * pickup.alpha;
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, text, pos, col, 0f, screenPos * size, new Vector2(scale));
        }

        _pickups.RemoveAll(x => x.alpha <= 0);
    }

    private static Vector2 GetPosition(int i, Vector2 screenPos, float scale, Vector2 size)
    {
        Vector2 originPos = screenPos * new Vector2(Main.screenWidth - 200, Main.screenHeight - 80) + new Vector2(100, 40);
        float yOff = 20 * i * scale;

        if (screenPos.Y > 0.5f)
            yOff *= -1;

        Vector2 finalPos = originPos + new Vector2(0, yOff) + size / 2f;
        return finalPos;
    }
}