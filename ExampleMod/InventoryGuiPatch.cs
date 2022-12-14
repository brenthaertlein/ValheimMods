using HarmonyLib;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ExampleMod
{
    [HarmonyPatch(typeof(InventoryGui))]
    public static class InventoryGuiPatch
    {
        [UsedImplicitly]
        [HarmonyPatch(nameof(InventoryGui.Awake))]
        [HarmonyPostfix]
        public static void Awake_Postfix(InventoryGui __instance)
        {
            Main.Log(level: LogLevel.DEBUG, text: "Awake_Postfix");
            var tab = Auga.API.PlayerPanel_AddTab("tabID", null, "Space Powers", OnTabSelected);
            var content = tab.ContentGO;
            var layout = content.AddComponent<VerticalLayoutGroup>();

            layout.spacing = 10;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
            layout.childScaleWidth = false;
            layout.childScaleHeight = false;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;

            CreateButton(content.transform, "ActivateSpacePower", "Activate Space Power", () => { Debug.Log("Space power activated");  });
            CreateButton(content.transform, "ChangeSpacePower", "Change Space Power", () => { Debug.Log("Changed space power"); });
            CreateButton(content.transform, "LevelUpSpacePower", "Level Up Space Power", () => { Debug.Log("Leveled up space power"); });
            CreateButton(content.transform, "ConsumeSpacePower", "Consume Space Power", () => { Debug.Log("Consume space power"); });
        }

        private static Button CreateButton(Transform parent, string name, string text, UnityAction action = null)
        {
            var button = Auga.API.SmallButton_Create(parent.transform, name, text);
            var transform = button.transform as RectTransform;

            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 120);
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 60);

            if (action != null)
            {
                button.onClick.AddListener(action);
            }

            return button;
        }

        private static void OnTabSelected(int index)
        {
            Main.Log(String.Format("Space Powers Tab Selected; index {0}", index));
        }

    }
}
