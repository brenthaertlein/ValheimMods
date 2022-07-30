using System;
using System.Collections.Generic;
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

namespace ValheimMods
{
    [BepInPlugin(PluginId, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [BepInDependency("randyknapp.mods.auga", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.NotEnforced, VersionStrictness.Minor)]
    public class ExampleMod : BaseUnityPlugin
    {
        public ConfigEntry<bool> IsLoggingEnabled;

        private const string PluginId = "com.brenthaertlein.mods.valheim.example";
        private const string PluginName = "Example Mod";
        private const string PluginVersion = "0.0.1";

        private Harmony harmony;

        [UsedImplicitly]
        public void Awake()
        {
            harmony = new Harmony(PluginId);
#if DEBUG

            LogPatchedMethods();
            Debug.Log("Removing patches for " + PluginId);
            Harmony.UnpatchID(PluginId);
            Debug.Log("Patching " + PluginName);
#endif
            harmony.PatchAll();
            Debug.Log(PluginName + " patched");

            InventoryGui.m_instance.Awake();
        }

        [UsedImplicitly]
        public void OnDestroy()
        {
#if DEBUG

            Debug.Log("Unloading " + PluginName);
            LogPatchedMethods();
#endif
            harmony?.UnpatchSelf();
            Debug.Log(PluginName + " unloaded");
        }

        private void LogPatchedMethods()
        {
            var methods = harmony?.GetPatchedMethods();
            foreach (var method in methods) {
                Debug.Log(method.FullDescription());
            }

        }
    }
}
