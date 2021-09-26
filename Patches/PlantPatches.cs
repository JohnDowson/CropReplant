using HarmonyLib;
using System;
using System.Reflection;

namespace CropReplant.Patches
{
#pragma warning disable IDE0051 // Remove unused private members
    [HarmonyPatch(typeof(Plant), "GetHoverText")]
    static class PlantGetHoverText_Patch
    {
        private static string GetColour(double percentage)
	{
		string result = "#e74c3c";
		if (percentage >= 25.0 && percentage <= 50.0)
		{
			result = "#e67e22";
		}
		if (percentage >= 50.0 && percentage <= 75.0)
		{
			result = "#f1c40f";
		}
		if (percentage >= 75.0 && percentage <= 100.0)
		{
			result = "#27ae60";
		}
		return result;
	}
        static readonly FieldInfo StatusField = AccessTools.Field(typeof(Plant), "m_status");
        static string Postfix(string __result, Plant __instance)
        {
            var m_status = (int)StatusField.GetValue(__instance);
            bool is_healthy = m_status == 0; // This is fragile, but oh well, enum Plant.Status is private
            if (is_healthy)
            {
                DateTime d = new DateTime(__instance.m_nview.GetZDO().GetLong("plantTime", ZNet.instance.GetTime().Ticks));
                var timeSincePlanted = (ZNet.instance.GetTime() - d).TotalSeconds;
                var percentGrow = (int)(timeSincePlanted / __instance.m_growTimeMax * 100);
				string colour = GetColour(percentGrow);
				__result += $"\n<color={colour}>{percentGrow}% ~ {TimeSpan.FromSeconds(__instance.m_growTimeMax - timeSincePlanted).ToString(@"hh\:mm\:ss")}</color>";

            }

            return __result;
        }
    }
}
