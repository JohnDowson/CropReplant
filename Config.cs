using BepInEx.Configuration;
using Jotunn.Configs;
using Jotunn.Managers;
using UnityEngine;

namespace CropReplant
{
    public static class CRConfig
    {
        public static float range;
        public static bool multipick;
        public static bool displayGrowth;
        public static bool useDurability;
        public static bool useCustomReplantKey;
        public static ConfigEntry<KeyCode> customReplantKey;
        public static bool blockHarvestNoResources;
        public static bool replantSame;
        public static ButtonConfig replantKey;
        public static void Bind(CropReplant cr)
        {
            range = cr.Config.Bind("General", "MultipickRange", 2f, "Radius to pick crops in").Value;
            multipick = cr.Config.Bind("General", "MultipickEnable", true, "Enable picking crops in radius").Value;
            displayGrowth = cr.Config.Bind("General", "DisplayGrowth", true, "Enable growth % tooltip on plant.").Value;
            useDurability = cr.Config.Bind("General", "UseDurability", true, "Enable cultivator durability depletion when replanting.").Value;

            useCustomReplantKey = cr.Config.Bind("General", "UseCustomReplantKey", false, "Use a custom keybind instead of game's alternative attack bind.").Value;
            customReplantKey = cr.Config.Bind("General", "customReplantKey", KeyCode.H, "Custom keybind to use instead of of game's alternative attack bind.");

            blockHarvestNoResources = cr.Config.Bind("General", "blockHarvestNoResources", false, "Block harvest if no resource to replant.").Value;
            replantSame = cr.Config.Bind("General", "replantSame", false, "Replant same plant if cultivate is selected in cultivator menu.").Value;

            replantKey = new ButtonConfig
            {
                Name = "ReplantKey",
                Config = customReplantKey,
                HintToken = "Replant"
            };
            InputManager.Instance.AddButton(CropReplant.PluginGUID, replantKey);
        }
    }


}
