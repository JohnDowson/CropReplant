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
                if (player.HasCultivator() && __instance.Replantable())
                {
                    string seedNameLocalized;
                    if (CropReplant.seedName == "same")
                        seedNameLocalized = "the same crop";
                    else
                        seedNameLocalized = Localization.instance.Localize("$piece_" + CropReplant.seedName);
                    return __result +
                        $"\n[<color=yellow><b>{CRConfig.replantHotkey}</b></color>] Replant with " + seedNameLocalized +
                        $"\n[<color=yellow><b>{CRConfig.nextSeedHotkey}</b></color>] Choose different seed";
                }
            }
            return __result;
        }
    }

    [HarmonyPatch(typeof(Pickable), "Interact")]
    static class PickableInteract_Patch
    {
        static void Prefix(Pickable __instance, Humanoid character, bool repeat)
        {
            if (__instance.Replantable())
            {
                if (!character.IsPlayer() || __instance.m_picked)
                {
                    return;
                }

                var player = (Player)character; // Safe cast, we already know character must be player
                if (player.HasCultivator())
                {
                    if (CRConfig.multipick)
                    {
                        foreach (var crop in __instance.FindPickableOfKindInRadius(CRConfig.range))
                        {
                            crop.Replant(player, false);
                        }
                    }
                }
            }
        }
    }
}
