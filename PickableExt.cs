using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace CropReplant
{
    public static class PickableExt
    {
        public static void Replant(this Pickable pickable, Player player, bool replant)
        {
            if (!pickable.m_picked)
            {
                pickable.m_nview.InvokeRPC("Pick", new Object[] { });
            }
            else return;

            if (replant)
            {
                string seedName = CropReplant.seedMap.FirstOrDefault(s => pickable.name.StartsWith(s.Key)).Value;
                GameObject prefab = ZNetScene.instance.GetPrefab(seedName);
                Piece piece = prefab.GetComponent<Piece>();

                bool hasResources = player.HaveRequirements(piece, Player.RequirementMode.CanBuild);

                if (hasResources)
                {
                    UnityEngine.Object.Instantiate(prefab, pickable.transform.position, Quaternion.identity);
                    player.ConsumeResources(piece.m_resources, 1);
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
