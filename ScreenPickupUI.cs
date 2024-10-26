using System.Collections.Generic;
using Terraria.UI;

namespace PickupConfiguration;

internal class ScreenPickupUI : ModSystem
{
    public static ScreenPickupUI Instance => ModContent.GetInstance<ScreenPickupUI>();

    internal UserInterface ScreenInterface;

    public static void AddPickupText(PopupText text) => (ModContent.GetInstance<ScreenPickupUI>().ScreenInterface.CurrentState as ScreenPickupUIState).AddPickup(text);

    public override void Load()
    {
        if (!Main.dedServ)
        {
            ScreenInterface = new UserInterface();
            ScreenInterface.SetState(new ScreenPickupUIState());
        }
    }

    public static void SwitchUI(UIState state)
    {
        if (Instance.ScreenInterface is null)
            return;

        Instance.ScreenInterface.SetState(state);
    }

    public override void UpdateUI(GameTime gameTime) => ScreenInterface.Update(gameTime);

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));

        if (resourceBarIndex != -1)
        {
            layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                "PickupConfiguration: Pickup UI",
                delegate
                {
                    ScreenInterface.Draw(Main.spriteBatch, Main.gameTimeCache);
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
}
