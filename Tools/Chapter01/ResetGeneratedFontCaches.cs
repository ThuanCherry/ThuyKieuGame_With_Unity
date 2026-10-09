if (Application.isPlaying) throw new System.InvalidOperationException("Exit Play Mode before restoring generated font caches.");
// Both assets had empty dynamic glyph tables at the start of this task. Keep portable VietnameseTMP intact.
foreach (var path in new[] {"Assets/_Game/UI/Fonts/ChapterVietnamese.asset", "Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF - Fallback.asset"})
{
    var font = UnityEditor.AssetDatabase.LoadAssetAtPath<TMPro.TMP_FontAsset>(path);
    font.ClearFontAssetData(true);
    UnityEditor.EditorUtility.SetDirty(font);
    UnityEditor.AssetDatabase.SaveAssetIfDirty(font);
}
return "Incidental dynamic glyph caches cleared; required VietnameseTMP unchanged";
