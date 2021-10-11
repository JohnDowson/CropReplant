using BepInEx;
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
        public const string PluginVersion = "3.0.0";

        private static readonly Harmony harmony = new(typeof(CropReplant).GetCustomAttributes(typeof(BepInPlugin), false)
            .Cast<BepInPlugin>()
            .First()
            .GUID);



#pragma warning disable IDE0051 // Remove unused private members

        private void Awake()
        {
            CRConfig.Bind(this);

            PickableExt.ExtendPickableList(CRConfig.customReplantables);
            Logger.LogMessage($"Loaded {CRConfig.customReplantables.Count} custom plants");

            harmony.PatchAll();
        }

        private void OnDestroy()
        {
            harmony.UnpatchSelf();
        }
    }
}
