﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace CropReplant
{
    public static class PickableExt
    {
        public static readonly string[] replantableCrops = {
                "Pickable_Carrot",
                "Pickable_Turnip",
                "Pickable_Onion",
                "Pickable_SeedCarrot",
                "Pickable_SeedTurnip",
                "Pickable_SeedOnion",
                "Pickable_Barley",
                "Pickable_Flax",
        };
        public static readonly string[] seeds = {
            "sapling_carrot",
            "sapling_turnip",
            "sapling_onion",
            "sapling_seedcarrot",
            "sapling_seedturnip",
            "sapling_seedonion",
            "sapling_barley",
            "sapling_flax",
        };
        public static readonly Dictionary<string, string> pickablePlant = new()
        {
            { "Carrot", "sapling_carrot" },
            { "Turnip", "sapling_turnip" },
            { "Onion", "sapling_onion" },
            { "TurnipSeeds", "sapling_seedturnip" },
            { "OnionSeeds", "sapling_seedonion" },
            { "CarrotSeeds", "sapling_seedcarrot" },
            { "Barley", "sapling_barley" },
            { "Flax", "sapling_flax" },
        };

    public static bool Replantable(this Pickable pickable)
        {
            return System.Array.Exists(replantableCrops, s => pickable.name.StartsWith(s));
        }

        public static void Replant(this Pickable pickable, Player player)
        {
        if (!pickable.m_picked)
            {
                GameObject prefab = player.m_rightItem?.m_shared?.m_buildPieces?.GetSelectedPrefab();

                Piece piece = null;

                if (prefab.name == "cultivate_v2" & CRConfig.replantSame)
                {
                    bool keyExists = pickablePlant.ContainsKey(pickable.m_itemPrefab.name);
                    if (keyExists)
                    {
                        prefab = ZNetScene.instance.GetPrefab(pickablePlant[pickable.m_itemPrefab.name]);
                    }
                    else
                    {
                        return;
                    }
                }
                else if (prefab.name == "cultivate_v2") return;

                if (prefab != null)
                {
                    if (System.Array.Exists(seeds, s => prefab?.name == s)) 
                    {
                        piece = prefab.GetComponent<Piece>();
                    }
                }
                else return;

                bool hasResources = player.HaveRequirements(piece, Player.RequirementMode.CanBuild);

                if (hasResources)
                {
                    pickable.m_nview.InvokeRPC("Pick", new Object[] { });
                    UnityEngine.Object.Instantiate(prefab, pickable.transform.position, Quaternion.identity);
                    player.ConsumeResources(piece.m_resources, 1);
                    player.UseItemInHand();
                }
                else if (!CRConfig.blockHarvestNoResources)
                {
                    pickable.m_nview.InvokeRPC("Pick", new Object[] { });
                }
            }
        }
        public static List<Pickable> FindPickableOfKindInRadius(this Pickable pickable, float distance)
        {
            List<Pickable> pickableList = GameObject.FindObjectsOfType<Pickable>()
                .Where(p => (p.name == pickable.name &&
                Vector3.Distance(pickable.transform.position, p.transform.position) <= distance))
                .ToList();
            pickableList.Remove(pickable);
            return pickableList;
        }
    }
}