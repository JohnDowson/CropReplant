using BepInEx;
using BepInEx.Configuration;
using DebugUtils;
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
        public static ConfigEntry<KeyCode> replantHotkey;
        public static ConfigEntry<KeyCode> nextSeedHotkey;

        public static readonly string[] replantableCrops = {
                "Pickable_Carrot",
                "Pickable_Turnip",
                "Pickable_SeedCarrot",
                "Pickable_SeedTurnip",
                "Pickable_Barley",
                "Pickable_Flax",
        };
        public static readonly Dictionary<string, string> seedMap = new Dictionary<string, string>
        {
            {"Pickable_Carrot", "sapling_carrot" },
            {"Pickable_Turnip", "sapling_turnip" },
            {"Pickable_SeedCarrot", "sapling_seedcarrot" },
            {"Pickable_SeedTurnip", "sapling_seedturnip" },
            {"Pickable_Barley", "sapling_barley" },
            {"Pickable_Flax", "sapling_flax" },
        };
        public static readonly string[] seeds =
        {
            "same",
            "sapling_carrot",
            "sapling_turnip",
            "sapling_seedcarrot",
            "sapling_seedturnip",
            "sapling_barley",
            "sapling_flax",
        };

        private static readonly int totalSeedOptions = seeds.Length;
        private static int seedCycle = 0;
        public static string seedName = "same";
        public static void NextSeed()
        {
            if (seedCycle < totalSeedOptions - 1)
                seedCycle++;
            else
                seedCycle = 0;

            seedName = seeds[seedCycle];
        }



#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE0060 // Remove unused parameter
        private void Awake()
        {
            range = Config.Bind("General", "MultipickRange", 2f, "Radius to pick crops in");
            multipick = Config.Bind("General", "MultipickEnable", true, "Enable picking crops in radius");
            replantHotkey = Config.Bind("General", "ReplantHotkey", KeyCode.H, "Hotkey for replanting");
            nextSeedHotkey = Config.Bind("General", "NextSeedHotkey", KeyCode.J, "Hotkey for scrolling through replant options");

            harmony.PatchAll();
        }

        private void OnDestroy()
        {
            harmony.UnpatchAll();
        }

        [HarmonyPatch(typeof(Pickable), "Interact")]
        static class PickableInteract_Patch
        {
            static void Prefix(Pickable __instance, Humanoid character, bool repeat)
            {
                if (__instance.Replantable())
                {
                    if (!character.IsPlayer() || __instance.m_picked)
                    {
                        return;
                    }

                    var player = (Player)character; // Safe cast, we already know character must be player
                    if (player.HasCultivator())
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

        [HarmonyPatch(typeof(Pickable), "GetHoverText")]
        static class PickableGetHoverText_Patch
        {
            static string Postfix(string __result, Pickable __instance)
            {
                if (!__instance.m_picked)
                {
                    var player = Player.m_localPlayer;
                    if (player.HasCultivator() && __instance.Replantable())
                    {
                        string seedNameLocalized;
                        if (seedName == "same")
                            seedNameLocalized = "same crop";
                        else
                            seedNameLocalized = Localization.instance.Localize("$piece_" + seedName);
                        return __result +
                            $"\n[<color=yellow><b>{replantHotkey.Value}</b></color>] Replant with {seedNameLocalized}" +
                            $"\n[<color=yellow><b>{nextSeedHotkey.Value}</b></color>] Choose another seed";
                    }
                }
                return __result;
            }
        }
        [HarmonyPatch(typeof(Player), "Update")]
        static class PlayerUpdate_Patch
        {
            static void Postfix(Player __instance)
            {
                var player = __instance;
                bool keyNextSeedDown = Input.GetKeyDown(nextSeedHotkey.Value);
                bool keyReplantDown = Input.GetKeyDown(replantHotkey.Value);
                if (keyReplantDown)
                {
                    if (player.HasCultivator())
                    {
                        var maybe_pickable = player.GetHoverObject()?.GetComponent<Pickable>();
                        if (maybe_pickable != null)
                        {
                            if (maybe_pickable.Replantable())
                            {
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
                if (keyNextSeedDown)
                {
                    NextSeed();
                }
            }
        }
    }
}
