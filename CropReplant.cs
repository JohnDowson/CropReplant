using BepInEx;
using HarmonyLib;
using System.Linq;

namespace CropReplant
{
    [BepInPlugin("com.github.johndowson.CropReplant", "CropReplant", "2.1.0")]
    public class CropReplant : BaseUnityPlugin
    {
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
