var current = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if(current.isDirty) UnityEditor.SceneManagement.EditorSceneManager.SaveScene(current, string.IsNullOrEmpty(current.path) ? UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/_Game/Scenes/Dev_ThuyKieuEditorBackup.unity") : current.path);
var report = new System.Collections.Generic.List<object>();
foreach(var path in new[]{"Assets/_Game/Scenes/Dev_PlayerMovement.unity","Assets/_Game/Scenes/KieuHome_Exterior.unity","Assets/_Game/Scenes/CoreTest.unity"}) {
 var s=UnityEditor.SceneManagement.EditorSceneManager.OpenScene(path,UnityEditor.SceneManagement.OpenSceneMode.Additive);
 report.Add(new{path,players=s.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<ThuyKieu.Player.PlayerMovement>(true)).Select(p=>new{p.name,position=p.transform.position.ToString(),animators=p.GetComponentsInChildren<UnityEngine.Animator>(true).Select(a=>new{a.name,avatar=a.avatar==null?null:a.avatar.name,controller=a.runtimeAnimatorController==null?null:a.runtimeAnimatorController.name}).ToArray()}).ToArray(),cameras=s.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<UnityEngine.Camera>(true)).Select(c=>new{c.name,components=c.GetComponents<UnityEngine.Component>().Select(x=>x.GetType().Name).ToArray()}).ToArray()});
 UnityEditor.SceneManagement.EditorSceneManager.CloseScene(s,true);
}
return report;
