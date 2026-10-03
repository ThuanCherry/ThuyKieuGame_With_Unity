if (UnityEditor.EditorApplication.isPlaying) return "Stop Play Mode before scene audit";
var report = new System.Collections.Generic.List<object>();
var original = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
foreach (string path in UnityEditor.AssetDatabase.FindAssets("t:Scene", new[]{"Assets/_Game/Scenes","Assets/Scenes"}).Select(UnityEditor.AssetDatabase.GUIDToAssetPath)) {
 var scene=UnityEngine.SceneManagement.SceneManager.GetSceneByPath(path);
 bool wasLoaded=scene.IsValid() && scene.isLoaded;
 if(!wasLoaded) scene=UnityEditor.SceneManagement.EditorSceneManager.OpenScene(path,UnityEditor.SceneManagement.OpenSceneMode.Additive);
 var roots=scene.GetRootGameObjects();
 report.Add(new{path, roots=roots.Select(r=>r.name).ToArray(), players=roots.SelectMany(r=>r.GetComponentsInChildren<ThuyKieu.Player.PlayerMovement>(true)).Count(), cameras=roots.SelectMany(r=>r.GetComponentsInChildren<UnityEngine.Camera>(true)).Count(), dialogueManagers=roots.SelectMany(r=>r.GetComponentsInChildren<ThuyKieu.Dialogue.DialogueManager>(true)).Count(), missingScripts=roots.SelectMany(r=>r.GetComponentsInChildren<UnityEngine.Transform>(true)).Sum(t=>UnityEditor.GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject))});
 if(!wasLoaded) UnityEditor.SceneManagement.EditorSceneManager.CloseScene(scene,true);
}
UnityEditor.SceneManagement.EditorSceneManager.SetActiveScene(original);
System.IO.File.WriteAllText("Tools/Chapter01/SceneAudit.json",Newtonsoft.Json.JsonConvert.SerializeObject(report,Newtonsoft.Json.Formatting.Indented));
return report;
