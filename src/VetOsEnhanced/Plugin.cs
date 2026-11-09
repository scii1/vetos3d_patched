using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace VetOsEnhanced
{
    [BepInPlugin("com.vetos.enhanced", "VetOs Enhanced Core", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public const string ModGUID = "com.vetos.enhanced";
        public const string ModName = "VetOs Enhanced Core";
        public const string ModVersion = "1.0.0";

        internal static ManualLogSource Log;
        private Harmony _harmony;

        private void Awake()
        {
            Log = Logger;
            Log.LogInfo("VetOs Enhanced Mod Altyapısı Aktif!");

            _harmony = new Harmony(ModGUID);
            _harmony.PatchAll();
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}
