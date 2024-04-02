using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace Ultrakill64
{
    [HarmonyPatch]
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private Harmony harmony;

        private static ConfigEntry<bool> enableFiltering;
        public const string NAME = "Ultrakill64";
        public const string VERSION = "1.0.1";
        public const string GUID = "Hydraxous.ULTRAKILL.Ultrakill64";

        private void Awake()
        {
            // Plugin startup logic
            harmony = new Harmony(GUID);
            harmony.PatchAll();
            
            enableFiltering = Config.Bind("General", "Enabled", true, "Enable N64 texture filtering");
            enableFiltering.SettingChanged += (_,_) => SetEffect(enableFiltering.Value);

            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        [HarmonyPatch(typeof(PostProcessV2_Handler), nameof(PostProcessV2_Handler.Fooled)), HarmonyPrefix]
        private static bool OnFooled(PostProcessV2_Handler __instance)
        {
            SetEffect(enableFiltering.Value);
            return false;
        }

        private static void SetEffect(bool enabled)
        {
            if (enabled)
                Shader.EnableKeyword("Fooled");
            else
                Shader.DisableKeyword("Fooled");
        }

    }
}
