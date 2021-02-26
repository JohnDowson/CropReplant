namespace CropReplant
{
    public static class PlayerExt
    {
        public static bool HasCultivator(this Player player)
        {
            return player.m_inventory.HaveItem("$item_cultivator");
        }
    }
}
