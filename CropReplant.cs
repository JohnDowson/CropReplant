using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CropReplant
{
    [BepInPlugin("jd.CropReplant", "CropReplant", "0.1.5")]
    public class CropReplant : BaseUnityPlugin
    {
        private static readonly Harmony harmony = new(typeof(CropReplant).GetCustomAttributes(typeof(BepInPlugin), false)
            .Cast<BepInPlugin>()
            .First()
            .GUID);

        public static ConfigEntry<float> range;
        public static ConfigEntry<bool> multipick;
        public static ConfigEntry<KeyCode> hotkey;

        public static Dictionary<string, string> seedMap = new Dictionary<string, string>
        {
            {"Pickable_Carrot", "sapling_carrot" },
            {"Pickable_Turnip", "sapling_turnip" },
            {"Pickable_SeedCarrot", "sapling_seedcarrot" },
            {"Pickable_SeedTurnip", "sapling_seedturnip" },
            {"Pickable_Barley", "sapling_barley" },
            {"Pickable_Flax", "sapling_flax" },
        };

#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE0060 // Remove unused parameter
        private void Awake()
        {
            range = Config.Bind<float>("General", "MultipickRange", 2f, "Radius to pick crops in");
            multipick = Config.Bind<bool>("General", "MultipickEnable", true, "Enable picking crops in radius");
            hotkey = Config.Bind<KeyCode>("General", "ReplantHotkey", KeyCode.H, "Hotkey for replanting");
            harmony.PatchAll();
        }

        private void OnDestroy()
        {
            harmony.UnpatchAll();
        }

        [HarmonyPatch(typeof(Pickable), "Interact")]
        static class Interact_Patch
        {
            static void Prefix(Pickable __instance, Humanoid character, bool repeat)
            {
                string crop_name = seedMap.FirstOrDefault(s => __instance.name.StartsWith(s.Key)).Key;
                if (crop_name != null)
                {
                    if (!character.IsPlayer() || __instance.m_picked)
                    {
                        return;
                    }

                    var player = (Player)character; // Safe cast, we already know character must be player
                    bool hasCultivator = player.m_inventory.HaveItem("$item_cultivator");
                    if (hasCultivator)
                    {
                        if (multipick.Value)
                        {
                            foreach (var crop in __instance.FindPickableOfKindInRadius(range.Value))
                            {
                                crop.Replant(player, false);
                            }
                        }
                    }
                }
            }
        }
        [HarmonyPatch(typeof(Player), "Update")]
        static class Update_Patch
        {
            static void Postfix(PlayerController __instance)
            {
                bool keyDown = Input.GetKeyDown(hotkey.Value);
                if (!keyDown)
                    return;
                var player = Player.m_localPlayer;
                bool hasCultivator = player.m_inventory.HaveItem("$item_cultivator");
                if (hasCultivator)
                {
                    DebugUtils.Logging.Log("hasCultivator");
                    var maybe_pickable = player.GetHoverObject()?.GetComponent<Pickable>();
                    if (maybe_pickable != null)
                    {
                        DebugUtils.Logging.Log("maybe_pickable != null");
                        string crop_name = seedMap.FirstOrDefault(s => maybe_pickable.name.StartsWith(s.Key)).Key;
                        if (crop_name != null)
                        {
                            DebugUtils.Logging.Log("crop_name != null");
                            maybe_pickable.Replant(player, true);
                            if (multipick.Value)
                            {
                                foreach (var crop in maybe_pickable.FindPickableOfKindInRadius(range.Value))
                                {
                                    crop.Replant(player, true);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
