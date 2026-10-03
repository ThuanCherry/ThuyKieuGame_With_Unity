return new {
    playing=UnityEditor.EditorApplication.isPlaying,
    environmentRunning=ThuyKieu.Environment.Editor.Chapter01EnvironmentCheck.Running,
    chapterRunning=ThuyKieu.Core.Editor.Chapter01PlayModeCheck.Running,
    scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene().path
};
