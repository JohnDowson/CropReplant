using BepInEx;
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
        public static void DBG(string str = "")
        {
              Debug.Log((typeof(CropReplant).Namespace + " ") + str);
        }
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
        private void Awake()
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }

        [HarmonyPatch(typeof(Pickable), "Drop")]
        static class Drop_Patch
        {
            static void Prefix(Pickable __instance)
            {
                string name = seedMap.FirstOrDefault(s => __instance.name.StartsWith(s.Key)).Value;
                if (name != null)
                {
                    GameObject prefab = ZNetScene.instance.GetPrefab(name);
                    Piece piece = prefab.GetComponent<Piece>();
                    Player player = Player.GetClosestPlayer(__instance.transform.position, 10f);
                    bool hasResources = player.HaveRequirements(piece, Player.RequirementMode.CanBuild);
                    bool hasCultivator = player.m_inventory.HaveItem("$item_cultivator");
                    if (hasResources && hasCultivator)
                    {
                        Instantiate(prefab, __instance.transform.position, Quaternion.identity);
                        player.ConsumeResources(piece.m_resources, 1);
                    }
                }
            }
        }
    }
}
