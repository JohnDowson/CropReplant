using HarmonyLib;

namespace CropReplant.PickablePatches
{
#pragma warning disable IDE0051 // Remove unused private members
    [HarmonyPatch(typeof(Pickable), "GetHoverText")]
    static class PickableGetHoverText_Patch
    {
        static string Postfix(string __result, Pickable __instance)
        {
            if (!__instance.m_picked)
            {
                var player = Player.m_localPlayer;
                if (player.CultivatorRequirement() && __instance.Replantable())
                {
                    string seedNameLocalized;
                    if (PickableExt.seedName == "same")
                        seedNameLocalized = "the same crop";
                    else
                        seedNameLocalized = Localization.instance.Localize("$piece_" + PickableExt.seedName);
                    return __result +
                        $"\n[<color=yellow><b>{CRConfig.customReplantKey}</b></color>] Replant with " + seedNameLocalized +
                        $"\n[<color=yellow><b>{CRConfig.nextSeedKey}</b></color>] Choose different seed";
                }
            }
            return __result;
        }
    }
}
