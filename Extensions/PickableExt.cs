using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace CropReplant
{
    public static class PickableExt
    {
        public static readonly string[] replantableCrops = {
                "Pickable_Carrot",
                "Pickable_Turnip",
                "Pickable_SeedCarrot",
                "Pickable_SeedTurnip",
                "Pickable_Barley",
                "Pickable_Flax",
        };
        public static readonly string[] seeds = {
            "sapling_carrot",
            "sapling_turnip",
            "sapling_seedcarrot",
            "sapling_seedturnip",
            "sapling_barley",
            "sapling_flax",
        };

        public static bool Replantable(this Pickable pickable)
        {
            return System.Array.Exists(replantableCrops, s => pickable.name.StartsWith(s));
        }

        public static void Replant(this Pickable pickable, Player player)
        {
            if (!pickable.m_picked)
            {
                pickable.m_nview.InvokeRPC("Pick", new Object[] { });
            }
            else return;

            GameObject prefab = player.m_rightItem.m_shared.m_buildPieces.GetSelectedPrefab();
            Piece piece;
            if (System.Array.Exists(seeds, s => prefab?.name == s))
                piece = prefab.GetComponent<Piece>();
            else return;

            bool hasResources = player.HaveRequirements(piece, Player.RequirementMode.CanBuild);

            if (hasResources)
            {
                UnityEngine.Object.Instantiate(prefab, pickable.transform.position, Quaternion.identity);
                player.ConsumeResources(piece.m_resources, 1);
                player.UseItemInHand();
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
