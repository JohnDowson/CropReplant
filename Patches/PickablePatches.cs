using HarmonyLib;

namespace CropReplant.PickablePatches
{
#pragma warning disable IDE0051 // Remove unused private members
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
                if (player.CultivatorEquipped())
                {
                    if (CRConfig.multipick)
                    {
                        foreach (var crop in __instance.FindPickableOfKindInRadius(CRConfig.range))
                        {
                            crop.Replant(player);
                        }
                    }
                }
            }
        }
    }
}
