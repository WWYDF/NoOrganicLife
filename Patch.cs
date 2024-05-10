using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoOrganicLife
{
    public class NOLConfig
    {
        public static ConfigEntry<bool> allowBlobs;
        public static ConfigEntry<bool> allowLiving;
    }

    [BepInPlugin(modID, modName, modVersion)]
    public class NoOrganicLife : BaseUnityPlugin
    {
        public static bool loaded;

        private const string modID = "Traveller.NoOrganicLife";

        private const string modName = "NoOrganicLife";

        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modID);

        private static NoOrganicLife Instance;

        public static ManualLogSource mls;

        private void Awake()
        {
            NOLConfig.allowBlobs = Config.Bind("Extra Allowed Enemies", "Allow Blobs?", true, "Whether or not the game should prevent Hygroderes from spawning inside.");
            NOLConfig.allowLiving = Config.Bind("Extra Allowed Enemies", "Allow Gray-Area Enemies?", false, "Whether or not the game should prevent Coil Heads, Nutcrackers, and Jesters from spawning inside.");

            mls = BepInEx.Logging.Logger.CreateLogSource(modName);
            mls.LogInfo("Loaded NoOrganicLife.");
            harmony.PatchAll(typeof(NoOrganicLife));
        }

       

        [HarmonyPatch(typeof(RoundManager), "LoadNewLevel")]
        [HarmonyPrefix]
        private static bool NoMonstersSpawn(ref SelectableLevel newLevel)
        {
            if (StartOfRound.Instance.currentLevel.PlanetName != "5 Embrion") // If planet is NOT Embrion, break.
                return true;

            mls.LogInfo("Landed on Embrion.");
            List<String> edgeCaseLiving = new List<String> { "Spring", "Nutcracker", "Jester" };
            foreach (SpawnableEnemyWithRarity Enemy in newLevel.Enemies) // Loop each possible enemy from the seeded enemy pool for current moon.
            {
                var LoopEnemy = Enemy.enemyType.enemyName;

                if (LoopEnemy.Contains("Blob")) // If selectedEnemy is a blob, run:
                {
                    if (NOLConfig.allowBlobs.Value) // If Blobs are enabled in config, run:
                        continue;
                }

                if (edgeCaseLiving.Any(variation => LoopEnemy.Contains(variation))) // If selectedEnemy is a part of the "edgeCaseLiving" whitelist, run:
                {
                    if (NOLConfig.allowLiving.Value) // If edge cases are enabled in config, run:
                        continue;
                }

                Enemy.rarity = 0; // Set Enemy rarity to 0. (disabled)
            }

            foreach (SpawnableEnemyWithRarity Enemy in newLevel.OutsideEnemies)
            {
                if (Enemy.enemyType.enemyName.Contains("RadMech"))
                    continue;

                Enemy.rarity = 0;
            }

            mls.LogDebug($"{modName}: Removed All Configured Organic Entities.");
            return true;
        }
    }
}