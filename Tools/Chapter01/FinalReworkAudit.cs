var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
var transforms = scene.GetRootGameObjects().SelectMany(r=>r.GetComponentsInChildren<Transform>(true)).ToArray();
var missing = transforms.Sum(t=>UnityEditor.GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject));
var texts = transforms.Select(t=>t.GetComponent<TMPro.TMP_Text>()).Where(t=>t!=null).ToArray();
var font = UnityEditor.AssetDatabase.LoadAssetAtPath<TMPro.TMP_FontAsset>("Assets/Fonts/VietnameseTMP.asset");
return new {scene=scene.path,playing=Application.isPlaying,dirty=scene.isDirty,missingScripts=missing,allSceneTMPUsesVietnamese=texts.All(t=>t.font==font),buildScenes=UnityEditor.EditorBuildSettings.scenes.Select(s=>new{s.path,s.enabled,exists=System.IO.File.Exists(s.path)}).ToArray(),fontCacheMethods=typeof(TMPro.TMP_FontAsset).GetMethods().Where(m=>m.Name=="ClearFontAssetData").Select(m=>m.ToString()).ToArray()};
