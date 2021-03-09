using HarmonyLib;
using System.Reflection;

namespace CropReplant
{
    public static class PlayerExt
    {
        public static MethodInfo FindHoverObject = typeof(Player).GetMethod("FindHoverObject", AccessTools.allDeclared);
        public static bool CultivatorEquipped(this Player player)
        {
            return player.m_rightItem?.m_shared.m_name == "$item_cultivator";
        }
        public static void UseItemInHand(this Player player)
        {
            player.m_rightItem.m_durability -= player.m_rightItem.m_shared.m_useDurabilityDrain;
        }

    }
}
