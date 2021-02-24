using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CropReplant
{
    [BepInPlugin("jd.CropReplant", "Tree Respawn", "0.1.0")]
    public class CropReplant : BaseUnityPlugin
    {

        public static ConfigEntry<float> range;
        public static ConfigEntry<bool> multipick;

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

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }

        public static Pickable[] FindPickableOfKindInRadius(string kind, Vector3 position, float distance)
        {
            return GameObject.FindObjectsOfType<Pickable>()
                .Where(p => (p.name.StartsWith(kind) &&
                Vector3.Distance(position, p.transform.position) <= distance)).ToArray();
        }



        [HarmonyPatch(typeof(Pickable), "Interact")]
        static class Interact_Patch
        {
            static void Prefix(Pickable __instance, Humanoid character, bool repeat)
            {
                KeyValuePair<string, string> map = seedMap.FirstOrDefault(s => __instance.name.StartsWith(s.Key));
                string crop_name = map.Key;
                string seed_name = map.Value;
                if (seed_name != null)
                {
                    if (!character.IsPlayer() || __instance.m_picked)
                    {
                        return;
                    }

                    Player player = (Player)character; // Safe cast, we already know character must be player
                    __instance.Replant(player, false);
                    if (multipick.Value)
                    {
                        foreach (Pickable crop in FindPickableOfKindInRadius(crop_name, __instance.transform.position, range.Value))
                        {
                            crop.Replant(player, true);
                        }
                    }
                }
            }
        }
    }
}
