using HarmonyLib;
using System.Reflection;

namespace CropReplant
{
    public static class PlayerExt
    {
        public static bool CultivatorEquipped(this Player player)
        {
            if (CRConfig.oldStyle)
                return player.m_inventory.HaveItem("$item_cultivator");
            return player.m_rightItem?.m_shared.m_name == "$item_cultivator";
        }
        public static void UseCultivatorDurability(this Player player)
        {
            var item = player.m_inventory.GetItem("$item_cultivator");
            item.m_durability -= item.m_shared.m_useDurabilityDrain;
        }

    }
}
