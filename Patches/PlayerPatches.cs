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
            bool keyReplantDown = ZInput.GetButtonDown("Remove") || ZInput.GetButtonDown("JoyRemove");
            if (keyReplantDown)
            {
                if (__instance.CultivatorEquipped())
                {
                    GameObject[] parameters = new GameObject[] { null, null };
                    PlayerExt.FindHoverObject.Invoke(__instance, parameters);
                    var maybePickable = parameters[0]?.GetComponent<Pickable>();
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
