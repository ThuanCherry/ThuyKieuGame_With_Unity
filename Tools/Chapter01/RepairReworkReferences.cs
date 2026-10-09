var scenes = UnityEditor.EditorBuildSettings.scenes;
foreach (var entry in scenes)
{
    if (System.IO.File.Exists(entry.path)) continue;
    var actual = UnityEditor.AssetDatabase.GUIDToAssetPath(entry.guid.ToString());
    if (!string.IsNullOrEmpty(actual)) entry.path = actual;
}
UnityEditor.EditorBuildSettings.scenes = scenes;
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
var font = UnityEditor.AssetDatabase.LoadAssetAtPath<TMPro.TMP_FontAsset>("Assets/Fonts/VietnameseTMP.asset");
foreach (var text in scene.GetRootGameObjects().SelectMany(r => r.GetComponentsInChildren<TMPro.TMP_Text>(true)))
{
    if (text.font == font) continue;
    text.font = font;
    text.fontSharedMaterial = font.material;
}
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
return UnityEditor.EditorBuildSettings.scenes.Select(s => s.path).ToArray();
