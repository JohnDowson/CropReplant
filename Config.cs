using UnityEngine;

namespace CropReplant
{
    public static class CRConfig
    {
        public static float range;
        public static bool multipick;
        public static KeyCode replantHotkey;
        public static KeyCode nextSeedHotkey;
        public static void Bind(CropReplant cr)
        {
            range = cr.Config.Bind("General", "MultipickRange", 2f, "Radius to pick crops in").Value;
            multipick = cr.Config.Bind("General", "MultipickEnable", true, "Enable picking crops in radius").Value;
            replantHotkey = cr.Config.Bind("General", "ReplantHotkey", KeyCode.H, "Hotkey for replanting").Value;
            nextSeedHotkey = cr.Config.Bind("General", "NextSeedHotkey", KeyCode.J, "Hotkey for scrolling through replant options").Value;
        }
    }


}
