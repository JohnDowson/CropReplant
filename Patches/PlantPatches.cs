using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
namespace CropReplant.Patches
{
#pragma warning disable IDE0051 // Remove unused private members
    [HarmonyPatch(typeof(Plant), "GetHoverText")]
    static class PlantGetHoverText_Patch
    {
		private static string GetColour(float percentage)
		{
			var red = new Color(0xE7, 0x4C, 0x3C);
            var green = new Color(0x27, 0xAE, 0x60);
            var color = Color.Lerp(red, green, percentage);
            return ColorUtility.ToHtmlStringRGBA(color);
		}
        static string Postfix(string __result, Plant __instance)
        {
            bool is_healthy = __instance.GetStatus() == Plant.Status.Healthy;
            if (is_healthy)
            {
                DateTime d = new(__instance.m_nview.GetZDO().GetLong("plantTime", ZNet.instance.GetTime().Ticks));
				var timeSincePlanted = (ZNet.instance.GetTime() - d).TotalSeconds;
				var growTime = __instance.GetGrowTime();
				float growProgress = (int)(timeSincePlanted / growTime);
                string colour = GetColour(growProgress);
                int growPercent = (int)(growProgress * 100f);
				if (growPercent < 100) 
				{
					__result += $"\n<color={colour}>{growPercent}% - {TimeSpan.FromSeconds(growTime - timeSincePlanted):hh\\:mm\\:ss}</color>";
				}
				if (growPercent == 100)
                {
                    __result += $"\n<color={colour}>{growPercent}%</color>";
				}
            }

            return __result;
        }
    }
}