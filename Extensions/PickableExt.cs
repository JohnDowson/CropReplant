using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace CropReplant
{
    public static class PickableExt
    {
        public static Dictionary<string, string> pickablePlants = new()
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

        public static void ExtendPickableList(Dictionary<string, string> extension)
        {
            foreach (var kv in extension.AsEnumerable())
            {
                pickablePlants.Append(kv);
            }
        }

        public static bool Replantable(this Pickable pickable)
        {
            return System.Array.Exists(pickablePlants.Keys.ToArray(), k => pickable.name.EndsWith(k));
        }

        public static void Replant(this Pickable pickable, Player player)
        {
            if (!pickable.m_picked)
            {
                GameObject prefab;
                if (CRConfig.oldStyle || CRConfig.replantSame)
                {
                    bool keyExists = pickablePlants.ContainsKey(pickable.m_itemPrefab.name);
                    if (keyExists)
                        prefab = ZNetScene.instance.GetPrefab(pickablePlants[pickable.m_itemPrefab.name]);
                    else
                        return;
                }
                else
                {
                    prefab = player.m_rightItem?.m_shared?.m_buildPieces?.GetSelectedPrefab();
                }
                Piece piece = prefab.GetComponent<Piece>();
                bool hasResources = player.HaveRequirements(piece, Player.RequirementMode.CanBuild);

                if (hasResources)
                {
                    pickable.m_nview.InvokeRPC("Pick", new Object[] { });
                    UnityEngine.Object.Instantiate(prefab, pickable.transform.position, Quaternion.identity);
                    player.ConsumeResources(piece.m_resources, 1);
                    if (CRConfig.useDurability)
                        player.UseCultivatorDurability();
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