using BepInEx;
using Jotunn;
using HarmonyLib;
using System.Linq;

namespace CropReplant
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    public class CropReplant : BaseUnityPlugin
    {
        public const string PluginGUID = "com.github.johndowson.CropReplant";
        public const string PluginName = "CropReplant";
        public const string PluginVersion = "2.3.0";

        private static readonly Harmony harmony = new(typeof(CropReplant).GetCustomAttributes(typeof(BepInPlugin), false)
            .Cast<BepInPlugin>()
            .First()
            .GUID);



#pragma warning disable IDE0051 // Remove unused private members

        private void Awake()
        {
            CRConfig.Bind(this);

            if (CRConfig.displayGrowth)
                harmony.PatchAll();
            else
            {
                harmony.PatchAll(typeof(PlayerPatches.PlayerUpdate_Patch));
            }
        }

        private void OnDestroy()
        {
            harmony.UnpatchSelf();
        }
    }
}
