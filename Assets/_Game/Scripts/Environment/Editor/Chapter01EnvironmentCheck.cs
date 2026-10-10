namespace ThuyKieu.Environment.Editor
{
    // Keep the original environment entry point on the current scene's physical traversal suite.
    public static class Chapter01EnvironmentCheck
    {
        public static bool Running => ThuyKieu.Core.Editor.Chapter01ReworkPlayCheck.Running;
        public static void Start() => ThuyKieu.Core.Editor.Chapter01ReworkPlayCheck.Start();
    }
}
