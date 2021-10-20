using HarmonyLib;
using UnityEngine;
using System.Reflection;

namespace CropReplant
{
    public static class PlayerExt
    {
        public static MethodInfo FindHoverObject = typeof(Player).GetMethod("FindHoverObject", AccessTools.allDeclared);
        public static bool CultivatorRequirement(this Player player)
        {
            if (CRConfig.oldStyle)
                return player.m_inventory.HaveItem("$item_cultivator");
            return player.m_rightItem?.m_shared.m_name == "$item_cultivator";
        }
        public static void UseItemInHand(this Player player)
        {
            if (CRConfig.useDurability & !CRConfig.oldStyle)
            {
                player.m_rightItem.m_durability -= player.m_rightItem.m_shared.m_useDurabilityDrain;
            }
        }
    }
}
