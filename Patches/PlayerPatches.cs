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
                ? Input.GetKeyDown(CRConfig.customReplantKey)
                : (ZInput.GetButtonDown("Remove") || ZInput.GetButtonDown("JoyRemove"));

            if (CRConfig.oldStyle)
            {
                bool keyNextSeedDown = Input.GetKeyDown(CRConfig.nextSeedKey);
                if (keyNextSeedDown)
                {
                    PickableExt.NextSeed();
                }
            }

            if (keyReplantDown)
            {
                if (__instance.CultivatorRequirement())
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