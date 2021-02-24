using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace CropReplant
{
    public static class PickableExt
    {
        public static void Replant(this Pickable pickable, Player player, bool chained)
        {
            if (chained)
            {
                if (!pickable.m_picked)
                {
                    pickable.RPC_Pick(0);
                }
                else return;
            }

            Util.DBG($"Replanting {pickable.name} {(chained ? "chained replant" : "")}");
            string seedName = CropReplant.seedMap.FirstOrDefault(s => pickable.name.StartsWith(s.Key)).Value;
            GameObject prefab = ZNetScene.instance.GetPrefab(seedName);
            Piece piece = prefab.GetComponent<Piece>();

            bool hasResources = player.HaveRequirements(piece, Player.RequirementMode.CanBuild);
            bool hasCultivator = player.m_inventory.HaveItem("$item_cultivator");

            if (hasResources && hasCultivator)
            {
                UnityEngine.Object.Instantiate(prefab, pickable.transform.position, Quaternion.identity);
                player.ConsumeResources(piece.m_resources, 1);
            }
        }
    }
}
