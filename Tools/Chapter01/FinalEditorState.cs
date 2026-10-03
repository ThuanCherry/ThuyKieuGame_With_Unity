if (UnityEditor.EditorApplication.isPlaying) return "Waiting for Edit Mode";
var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if(scene.path!=ThuyKieu.Core.Editor.Chapter01Setup.ScenePath)
 scene=UnityEditor.SceneManagement.EditorSceneManager.OpenScene(ThuyKieu.Core.Editor.Chapter01Setup.ScenePath);
var buildScenes=UnityEditor.EditorBuildSettings.scenes.ToList();
var chapter=buildScenes.First(s=>s.path==ThuyKieu.Core.Editor.Chapter01Setup.ScenePath);
buildScenes.Remove(chapter); chapter.enabled=true; buildScenes.Insert(0,chapter);
UnityEditor.EditorBuildSettings.scenes=buildScenes.ToArray();
return new{scene=scene.path,scene.isDirty,playing=UnityEditor.EditorApplication.isPlaying,buildEntry=UnityEditor.EditorBuildSettings.scenes[0].path,players=UnityEngine.Object.FindObjectsByType<ThuyKieu.Player.PlayerMovement>().Length,missingScripts=scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<UnityEngine.Transform>(true)).Sum(t=>UnityEditor.GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject))};
