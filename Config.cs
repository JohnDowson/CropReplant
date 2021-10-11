using BepInEx.Configuration;
using Jotunn.Configs;
using Jotunn.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CropReplant
{
    public static class CRConfig
    {
        public static ConfigEntry<float> c_range;
        public static ConfigEntry<bool> c_multipick;
        public static ConfigEntry<bool> c_displayGrowth;
        public static ConfigEntry<bool> c_useDurability;
        public static ConfigEntry<bool> c_blockHarvestNoResources;
        public static ConfigEntry<bool> c_replantSame;
        public static ConfigEntry<string> c_customReplantables;
        public static ConfigEntry<bool> c_oldStyle;

        public static float range => c_range.Value;
        public static bool multipick => c_multipick.Value;
        public static bool displayGrowth => c_displayGrowth.Value;
        public static bool useDurability => c_useDurability.Value;
        public static bool blockHarvestNoResources => c_blockHarvestNoResources.Value;
        public static bool replantSame => c_replantSame.Value;
        public static Dictionary<string, string> customReplantables;
        public static bool oldStyle => c_oldStyle.Value;

        public static ConfigEntry<bool> c_useCustomReplantKey;
        public static ConfigEntry<KeyCode> c_customReplantKey;

        public static bool useCustomReplantKey => c_useCustomReplantKey.Value;
        public static KeyCode customReplantKey => c_customReplantKey.Value;
        public static ButtonConfig replantButton;
        public static KeyHintConfig replantHint;

        public static Dictionary<string, string> ParseReplantables(string replantables)
        {
            if (System.String.IsNullOrEmpty(replantables))
                return new Dictionary<string, string> { };

            return replantables
                .Split(',')
                .Select(s => s.Split(':').Select(s => s.Trim()))
                .ToDictionary(s => s.First(), s => s.ElementAt(1));
        }

        public static void Bind(CropReplant cr)
        {
            c_range = cr.Config.Bind("General", "MultipickRange", 2f, "Radius to pick crops in");
            c_multipick = cr.Config.Bind("General", "MultipickEnable", true, "Enable picking crops in radius");

            c_displayGrowth = cr.Config.Bind("General", "DisplayGrowth", true, "Enable growth % tooltip on plant.");

            c_useDurability = cr.Config.Bind("General", "UseDurability", true, "Enable cultivator durability depletion when replanting.");

            c_blockHarvestNoResources = cr.Config.Bind("General", "blockHarvestNoResources", false, "Block harvest if no resource to replant.");
            c_replantSame = cr.Config.Bind("General", "replantSame", false, "Replant same plant if cultivate is selected in cultivator menu.");

            c_customReplantables = cr.Config.Bind("General", "customReplantables", "", "Custom plants to replant in format 'SeedName : PlantName, SeedName : PlantName'");
            customReplantables = ParseReplantables(c_customReplantables.Value);

            c_oldStyle = cr.Config.Bind("General", "oldStyle", false, "1.6 style replanting");

            c_useCustomReplantKey = cr.Config.Bind("General", "UseCustomReplantKey", false, "Use a custom keybind instead of game's alternative attack bind.");

            c_customReplantKey = cr.Config.Bind("General", "customReplantKey", KeyCode.H, "Custom keybind to use instead of of game's alternative attack bind.");
            replantButton = new ButtonConfig
            {
                Name = "ReplantKey",
                Config = c_customReplantKey,
                HintToken = "Replant"
            };
            replantHint = new KeyHintConfig
            {
                ButtonConfigs = new ButtonConfig[] { replantButton },
            };
            GUIManager.Instance.AddKeyHint(replantHint);
            InputManager.Instance.AddButton(CropReplant.PluginGUID, replantButton);

        }
    }


}
