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

            bool keyReplantDown = CRConfig.useCustomReplantKey
                ? Input.GetKeyDown(CRConfig.customReplantKey)
                : (ZInput.GetButtonDown("Remove") || ZInput.GetButtonDown("JoyRemove"));
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
                                    GameObject prefab = __instance.m_rightItem?.m_shared?.m_buildPieces?.GetSelectedPrefab();

                                    Piece piece = null;
                                    if (prefab != null)
                                    {
                                        if (System.Array.Exists(PickableExt.seeds, s => prefab?.name == s))
                                        {
                                            piece = prefab.GetComponent<Piece>();
                                            bool hasResources = __instance.HaveRequirements(piece, Player.RequirementMode.CanBuild);
                                            if (hasResources)
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
        }
    }
}
