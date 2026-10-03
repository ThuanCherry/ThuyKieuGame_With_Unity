using System;
using ThuyKieu.Dialogue;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ThuyKieu.UI.Editor
{
    public static class Chapter01DialogueStyle
    {
        public static void Apply(DialogueUI ui)
        {
            var serialized = new SerializedObject(ui);
            var panel = ((GameObject)serialized.FindProperty("dialoguePanel").objectReferenceValue).GetComponent<RectTransform>();
            var speaker = (TMP_Text)serialized.FindProperty("_speakerText").objectReferenceValue;
            var body = (TMP_Text)serialized.FindProperty("_bodyText").objectReferenceValue;
            var next = (Button)serialized.FindProperty("continueButton").objectReferenceValue;
            var template = (Button)serialized.FindProperty("choiceButtonPrefab").objectReferenceValue;
            panel.anchorMin = new Vector2(.12f, .035f);
            panel.anchorMax = new Vector2(.88f, .035f);
            panel.pivot = new Vector2(.5f, 0);
            panel.offsetMin = Vector2.zero;
            panel.offsetMax = new Vector2(0, 250);
            panel.GetComponent<Image>().color = new Color(.035f, .075f, .13f, .62f);
            var outline = panel.GetComponent<Outline>();
            // Image Outline draws full rectangle copies and compounds a translucent fill.
            if (outline != null) outline.enabled = false;
            Border(panel, "BorderTop", new Vector2(0, 1), Vector2.one, new Vector2(0, -2), Vector2.zero);
            Border(panel, "BorderBottom", Vector2.zero, new Vector2(1, 0), Vector2.zero, new Vector2(0, 2));
            Border(panel, "BorderLeft", Vector2.zero, new Vector2(0, 1), Vector2.zero, new Vector2(2, 0));
            Border(panel, "BorderRight", new Vector2(1, 0), Vector2.one, new Vector2(-2, 0), Vector2.zero);
            speaker.color = new Color(1f, .85f, .54f);
            speaker.fontSize = 28;
            speaker.fontStyle = FontStyles.Bold;
            speaker.rectTransform.anchorMin = new Vector2(0, 1);
            speaker.rectTransform.anchorMax = Vector2.one;
            speaker.rectTransform.offsetMin = new Vector2(28, -54);
            speaker.rectTransform.offsetMax = new Vector2(-28, -12);
            body.color = new Color(.98f, .98f, .94f);
            var textMaterial = ShadowMaterial(body);
            speaker.fontSharedMaterial = textMaterial;
            body.fontSharedMaterial = textMaterial;
            body.fontSize = 27;
            body.margin = Vector4.zero;
            body.enableAutoSizing = false;
            body.rectTransform.anchorMin = Vector2.zero;
            body.rectTransform.anchorMax = Vector2.one;
            body.rectTransform.offsetMin = new Vector2(28, 76);
            body.rectTransform.offsetMax = new Vector2(-28, -60);
            foreach (var button in new[] { next, template })
            {
                button.GetComponent<Image>().color = new Color(.12f, .22f, .34f, .8f);
                var colors = button.colors;
                colors.normalColor = Color.white;
                colors.highlightedColor = new Color(.73f, .88f, 1f);
                colors.selectedColor = colors.highlightedColor;
                colors.pressedColor = new Color(.55f, .72f, .89f);
                button.colors = colors;
                var label = button.GetComponentInChildren<TMP_Text>(true);
                label.color = new Color(.98f, .98f, .94f);
                label.fontSharedMaterial = textMaterial;
                label.fontSize = 24;
                label.enableAutoSizing = true;
                label.fontSizeMin = 20;
                label.fontSizeMax = 24;
            }
            var nextRect = next.GetComponent<RectTransform>();
            nextRect.anchorMin = new Vector2(1, 0);
            nextRect.anchorMax = new Vector2(1, 0);
            nextRect.offsetMin = new Vector2(-270, 20);
            nextRect.offsetMax = new Vector2(-28, 62);
            next.GetComponentInChildren<TMP_Text>(true).text = "Tiếp · Space";
            var size = template.GetComponent<LayoutElement>();
            size.minHeight = 48;
            size.preferredHeight = 48;
            var choices = (RectTransform)serialized.FindProperty("choicesRoot").objectReferenceValue;
            choices.GetComponent<VerticalLayoutGroup>().spacing = 8;
            EditorUtility.SetDirty(ui);
        }

        private static void Border(RectTransform panel, string name, Vector2 min, Vector2 max, Vector2 offsetMin, Vector2 offsetMax)
        {
            var existing = panel.Find(name);
            var rect = existing != null ? existing.GetComponent<RectTransform>()
                : new GameObject(name, typeof(RectTransform), typeof(Image)).GetComponent<RectTransform>();
            if (existing == null) rect.SetParent(panel, false);
            rect.anchorMin = min; rect.anchorMax = max;
            rect.offsetMin = offsetMin; rect.offsetMax = offsetMax;
            var image = rect.GetComponent<Image>();
            image.color = new Color(.69f, .83f, .96f, .95f);
            image.raycastTarget = false;
        }

        private static Material ShadowMaterial(TMP_Text text)
        {
            const string folder = "Assets/_Game/UI/Materials";
            const string path = folder + "/DialogueTextShadow.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                if (!AssetDatabase.IsValidFolder(folder)) AssetDatabase.CreateFolder("Assets/_Game/UI", "Materials");
                material = new Material(text.font.material) { name = "DialogueTextShadow" };
                AssetDatabase.CreateAsset(material, path);
            }
            // TMP's SDF underlay renders a shadow without changing the shared font material.
            material.EnableKeyword("UNDERLAY_ON");
            material.DisableKeyword("UNDERLAY_INNER");
            material.SetColor("_UnderlayColor", new Color(0, 0, 0, .9f));
            material.SetFloat("_UnderlayOffsetX", .65f);
            material.SetFloat("_UnderlayOffsetY", -.65f);
            material.SetFloat("_UnderlayDilate", .12f);
            material.SetFloat("_UnderlaySoftness", .2f);
            EditorUtility.SetDirty(material);
            AssetDatabase.SaveAssets();
            return material;
        }
    }
}
