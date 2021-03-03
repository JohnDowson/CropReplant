using HarmonyLib;
using UnityEngine;

namespace CropReplant.PlayerPatches
{
#pragma warning disable IDE0051 // Remove unused private members
    [HarmonyPatch(typeof(Player), "Update")]
    static class PlayerUpdate_Patch
    {
        static void Postfix(Player __instance)
        {
            bool keyNextSeedDown = Input.GetKeyDown(CRConfig.nextSeedHotkey);
            bool keyReplantDown = Input.GetKeyDown(CRConfig.replantHotkey);
            if (keyReplantDown)
            {
                if (__instance.HasCultivator())
                {
                    var maybe_pickable = __instance.GetHoverObject()?.GetComponent<Pickable>();
                    if (maybe_pickable != null)
                    {
                        if (maybe_pickable.Replantable())
                        {
                            maybe_pickable.Replant(__instance, true);
                            if (CRConfig.multipick)
                            {
                                foreach (var crop in maybe_pickable.FindPickableOfKindInRadius(CRConfig.range))
                                {
                                    crop.Replant(__instance, true);
                                }
                            }
                        }
                    }
                }
            }
            if (keyNextSeedDown)
            {
                CropReplant.NextSeed();
            }
        }
    }
}
