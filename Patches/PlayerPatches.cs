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

            bool keyReplantDown = (CRConfig.useCustomReplantKey || CRConfig.oldStyle)
                ? ZInput.GetButtonDown(CRConfig.replantButton.Name)
                : (ZInput.GetButtonDown("Remove") || ZInput.GetButtonDown("JoyRemove"));
            if (keyReplantDown)
            {
                if (__instance.CultivatorEquipped())
                {
                    __instance.FindHoverObject(out GameObject hover, out Character _);
                    var maybePickable = hover?.GetComponent<Pickable>();
                    if (maybePickable != null)
                    {
                        if (maybePickable.Replantable())
                        {
                            maybePickable.Replant(__instance);
                            if (CRConfig.multipick)
                            {
                                foreach (var crop in maybePickable.FindPickableOfKindInRadius(CRConfig.range))
                                {
                                    crop.Replant(__instance);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}