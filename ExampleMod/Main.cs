using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using Jotunn.Utils;
using UnityEngine;

namespace ExampleMod
{
    [BepInPlugin(PluginId, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [BepInDependency("randyknapp.mods.auga", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.NotEnforced, VersionStrictness.Minor)]
    public class Main: BaseUnityPlugin
    {
        public static ConfigEntry<bool> isLoggingEnabled;

        private const string PluginId = "com.brenthaertlein.mods.valheim.example";
        private const string PluginName = "Example Mod";
        private const string PluginVersion = "0.0.1";

        private Harmony harmony;

        [UsedImplicitly]
        public void Awake()
        {
            SetUp();
            harmony = new Harmony(PluginId);
            DebugMode(() =>
            {
                LogPatchedMethods();
                Log("Removing patches for " + PluginId);
                Harmony.UnpatchID(PluginId);
                Log("Patching " + PluginName);
            });
            harmony.PatchAll();
            Log(PluginName + " patched");
        }
        [UsedImplicitly]
        public void OnDestroy()
        {
            DebugMode(() =>
            {
                Log("Unloading " + PluginName);
                LogPatchedMethods();
            });
            harmony?.UnpatchSelf();
            TearDown();
            Log(PluginName + " unloaded");
        }

        public static void Log(string text, LogLevel level = LogLevel.DEBUG)
        {
            if (isLoggingEnabled?.Value ?? true)
            {
                var now = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);
                string format = "[{0}] [DEBUG] {1}";
                Debug.Log(String.Format(format, now, text));
            }
        }

        private void SetUp()
        {
            isLoggingEnabled = Config.Bind<bool>("Logging", "is_logging_enabled", true, "Toggles logging from this plugin");
        }
        private void TearDown()
        {

        }

        private void LogPatchedMethods()
        {
            var methods = harmony?.GetPatchedMethods();
            foreach (var method in methods)
            {
                Log(method.FullDescription());
            }

        }

        private void DebugMode(Action action)
        {
#if DEBUG
            action();
#endif
        }
    }
}
