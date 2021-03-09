namespace CropReplant
{
    public static class CRConfig
    {
        public static float range;
        public static bool multipick;
        public static bool displayGrowth;
        public static void Bind(CropReplant cr)
        {
            range = cr.Config.Bind("General", "MultipickRange", 2f, "Radius to pick crops in").Value;
            multipick = cr.Config.Bind("General", "MultipickEnable", true, "Enable picking crops in radius").Value;
            displayGrowth = cr.Config.Bind("General", "DisplayGrowth", true, "Enable growth % tooltip on plant.").Value;
        }
    }


}
