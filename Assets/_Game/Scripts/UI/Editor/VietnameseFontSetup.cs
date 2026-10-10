using System;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace ThuyKieu.UI.Editor
{
    public static class VietnameseFontSetup
    {
        public const string FontPath = "Assets/Fonts/VietnameseTMP.asset";

        public static string CharacterSet => new string(
            Enumerable.Range(32, 95).Concat(Enumerable.Range(0xA0, 0xE0))
                .Concat(Enumerable.Range(0x1EA0, 90))
                .Concat("\u01A0\u01A1\u01AF\u01B0".Select(c => (int)c))
                .Concat("\u0300\u0301\u0303\u0309\u0323\u0306\u0302\u031B–—‘’“”•…₫←↑→↓".Select(c => (int)c))
                .Distinct().Select(c => (char)c).ToArray());

        [MenuItem("ThuyKieu/Fonts/Create portable Vietnamese TMP")]
        public static void Create()
        {
            if (Application.isPlaying) throw new InvalidOperationException("Exit Play Mode before creating font assets.");
            AssetDatabase.Refresh();
            if (AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath) != null)
                throw new InvalidOperationException("VietnameseTMP already exists; validate or export it without replacing it.");
            var source = AssetDatabase.LoadAssetAtPath<Font>("Assets/Fonts/LiberationSans.ttf");
            if (source == null) throw new InvalidOperationException("Missing bundled LiberationSans.ttf.");
            var font = TMP_FontAsset.CreateFontAsset(source, 64, 8, GlyphRenderMode.SDFAA, 2048, 2048, AtlasPopulationMode.Dynamic);
            font.name = "VietnameseTMP";
            font.isMultiAtlasTexturesEnabled = false;
            if (!font.TryAddCharacters(CharacterSet, out string missing))
                throw new InvalidOperationException("Unable to bake required glyphs: " + missing);
            // Persist the complete atlas; rendering never depends on local installed fonts.
            font.atlasPopulationMode = AtlasPopulationMode.Static;
            AssetDatabase.CreateAsset(font, FontPath);
            font.material.name = "VietnameseTMP Material";
            AssetDatabase.AddObjectToAsset(font.material, font);
            foreach (var texture in font.atlasTextures)
            {
                texture.name = "VietnameseTMP Atlas";
                AssetDatabase.AddObjectToAsset(texture, font);
                EditorUtility.SetDirty(texture);
            }
            EditorUtility.SetDirty(font);
            EditorUtility.SetDirty(font.material);
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(FontPath, ImportAssetOptions.ForceUpdate);
            Validate();
            Export();
        }

        [MenuItem("ThuyKieu/Fonts/Repair portable Vietnamese TMP")]
        public static void Repair()
        {
            if (Application.isPlaying) throw new InvalidOperationException("Exit Play Mode before repairing fonts.");
            var font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
            var source = AssetDatabase.LoadAssetAtPath<Font>("Assets/Fonts/LiberationSans.ttf");
            if (font == null || source == null) throw new InvalidOperationException("Missing existing Vietnamese font or bundled source.");
            if (FontEngine.LoadFontFace(source, 64) != FontEngineError.Success)
                throw new InvalidOperationException("Unable to load bundled font face.");
            var unsupported = CharacterSet.Where(c => !FontEngine.TryGetGlyphWithUnicodeValue(c,
                GlyphLoadFlags.LOAD_NO_BITMAP, out _)).ToArray();
            if (unsupported.Length > 0) throw new InvalidOperationException("Source font lacks: " + new string(unsupported));

            // Rebuild all glyphs together: extending a cleared atlas invalidates old glyph rectangles.
            var serialized = new SerializedObject(font);
            serialized.FindProperty("m_SourceFontFile").objectReferenceValue = source;
            serialized.FindProperty("m_SourceFontFileGUID").stringValue = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(source));
            serialized.ApplyModifiedProperties();
            font.atlasPopulationMode = AtlasPopulationMode.Dynamic;
            font.ClearFontAssetData(false);
            if (!font.TryAddCharacters(CharacterSet, out string missing))
                throw new InvalidOperationException("Unable to bake required glyphs: " + missing);
            font.atlasPopulationMode = AtlasPopulationMode.Static;
            font.material.mainTexture = font.atlasTextures[0];
            EditorUtility.SetDirty(font);
            EditorUtility.SetDirty(font.material);
            foreach (var texture in font.atlasTextures) EditorUtility.SetDirty(texture);
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(FontPath, ImportAssetOptions.ForceUpdate);
            Validate();
        }

        [MenuItem("ThuyKieu/Fonts/Validate portable Vietnamese TMP")]
        public static void Validate()
        {
            var font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
            if (font == null) throw new InvalidOperationException("Create VietnameseTMP first.");
            var missing = CharacterSet.Where(c => !font.HasCharacter(c, false, false)).ToArray();
            if (missing.Length != 0) throw new InvalidOperationException("Missing baked characters: " + new string(missing));
            if (font.atlasPopulationMode != AtlasPopulationMode.Static || font.atlasTextures.Any(t => t == null))
                throw new InvalidOperationException("Font must contain a static atlas.");
            if (font.material.mainTexture != font.atlasTextures[0])
                throw new InvalidOperationException("Material does not reference the bundled atlas.");
            string sample = "Thúy Kiều – Mẹ Kiều – Mã Giám Sinh. CHƯƠNG 1: GIA BIẾN. Ă Â Đ Ê Ô Ơ Ư ă â đ ê ô ơ ư. Ắ Ằ Ẳ Ẵ Ặ Ấ Ầ Ẩ Ẫ Ậ Ế Ề Ể Ễ Ệ Ố Ồ Ổ Ỗ Ộ Ớ Ờ Ở Ỡ Ợ Ứ Ừ Ử Ữ Ự Ỳ Ý Ỷ Ỹ Ỵ.";
            string decomposed = sample.Normalize(NormalizationForm.FormD);
            if (decomposed.Any(c => !char.IsWhiteSpace(c) && !font.HasCharacter(c, false, false)))
                throw new InvalidOperationException("Missing decomposed Vietnamese combining marks.");
            Directory.CreateDirectory("Tools/Fonts");
            File.WriteAllText("Tools/Fonts/VietnameseFontReport.json", JsonUtility.ToJson(new Report
            {
                passed = true, requiredCharacters = CharacterSet.Length,
                atlasCount = font.atlasTextures.Length,
                atlasWidth = font.atlasTextures[0].width, atlasHeight = font.atlasTextures[0].height,
                staticAtlas = true, composedAndDecomposed = true, sample = sample
            }, true));
        }

        [MenuItem("ThuyKieu/Fonts/Export portable Vietnamese TMP")]
        public static void Export()
        {
            Validate();
            var assets = AssetDatabase.FindAssets("", new[] { "Assets/Fonts" }).Select(AssetDatabase.GUIDToAssetPath).ToArray();
            var dependencies = AssetDatabase.GetDependencies(assets, true).Where(path => path.StartsWith("Assets/", StringComparison.Ordinal));
            var shaderIncludes = AssetDatabase.FindAssets("", new[] { "Assets/TextMesh Pro/Shaders" })
                .Select(AssetDatabase.GUIDToAssetPath).Where(path => path.EndsWith(".cginc", StringComparison.Ordinal));
            // Share asset dependencies, never export source files from installed Unity packages.
            var paths = assets.Concat(dependencies).Concat(shaderIncludes).Append("Assets/Fonts").Distinct().ToArray();
            UnityEditor.AssetPackage.Package.Export(new UnityEditor.AssetPackage.ExportPackageParameters
            {
                AssetPathNames = paths, FileName = "Tools/Fonts/VietnameseTMP.unitypackage",
                Flags = ExportPackageOptions.Recurse
            });
        }

        [Serializable] private class Report
        {
            public bool passed, staticAtlas, composedAndDecomposed;
            public int requiredCharacters, atlasCount, atlasWidth, atlasHeight;
            public string sample;
        }
    }
}
