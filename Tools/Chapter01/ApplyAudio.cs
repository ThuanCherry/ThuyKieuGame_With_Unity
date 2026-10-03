if (UnityEditor.EditorApplication.isPlaying) throw new System.InvalidOperationException("Exit Play Mode first.");
var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
if (scene.path != ThuyKieu.Core.Editor.Chapter01Setup.ScenePath)
{
    if (scene.isDirty) throw new System.InvalidOperationException("Save the current scene before switching to Chapter 1.");
    UnityEditor.SceneManagement.EditorSceneManager.OpenScene(ThuyKieu.Core.Editor.Chapter01Setup.ScenePath);
}
UnityEditor.AssetDatabase.Refresh();
ThuyKieu.Environment.Editor.Chapter01AudioSetup.Apply();
return "Assigned nine audio clips and saved Chapter 1.";
