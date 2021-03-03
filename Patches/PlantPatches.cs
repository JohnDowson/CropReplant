using DebugUtils;
using HarmonyLib;
using System;
using System.Reflection;

namespace CropReplant.Patches
{
#pragma warning disable IDE0051 // Remove unused private members
    [HarmonyPatch(typeof(Plant), "GetHoverText")]
    static class PlantGetHoverText_Patch
    {
        static readonly FieldInfo StatusField = AccessTools.Field(typeof(Plant), "m_status");
        static string Postfix(string __result, Plant __instance)
        {
            var m_status = (int)StatusField.GetValue(__instance);
            bool is_healthy = m_status == 0; // This is fragile, but oh well, enum Plant.Status is private
            if (is_healthy)
            {
                DateTime d = new DateTime(__instance.m_nview.GetZDO().GetLong("plantTime", ZNet.instance.GetTime().Ticks));
                var timeSincePlanted = (ZNet.instance.GetTime() - d).TotalSeconds;
                __result += $"\n{(int)(timeSincePlanted/__instance.m_growTimeMax*100)}% grown";
            }

            return __result;
        }
    }
}
