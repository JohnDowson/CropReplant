using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace CropReplant
{
    [BepInPlugin("com.github.johndowson.CropReplant", "CropReplant", "0.1.5")]
    public class CropReplant : BaseUnityPlugin
    {
        private static readonly Harmony harmony = new(typeof(CropReplant).GetCustomAttributes(typeof(BepInPlugin), false)
            .Cast<BepInPlugin>()
            .First()
            .GUID);


        public static readonly Dictionary<string, string> seedMap = new Dictionary<string, string>
        {
            {"Pickable_Carrot", "sapling_carrot" },
            {"Pickable_Turnip", "sapling_turnip" },
            {"Pickable_SeedCarrot", "sapling_seedcarrot" },
            {"Pickable_SeedTurnip", "sapling_seedturnip" },
            {"Pickable_Barley", "sapling_barley" },
            {"Pickable_Flax", "sapling_flax" },
        };
        public static readonly string[] seeds =
        {
            "same",
            "sapling_carrot",
            "sapling_turnip",
            "sapling_seedcarrot",
            "sapling_seedturnip",
            "sapling_barley",
            "sapling_flax",
        };

        private static readonly int totalSeedOptions = seeds.Length;
        private static int seedCycle = 0;
        public static string seedName = "same";
        public static void NextSeed()
        {
            if (seedCycle < totalSeedOptions - 1)
                seedCycle++;
            else
                seedCycle = 0;

            seedName = seeds[seedCycle];
        }



#pragma warning disable IDE0051 // Remove unused private members

        private void Awake()
        {
            CRConfig.Bind(this);

            harmony.PatchAll();
        }

        private void OnDestroy()
        {
            harmony.UnpatchSelf();
        }




    }
}
