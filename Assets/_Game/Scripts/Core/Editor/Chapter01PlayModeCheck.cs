namespace ThuyKieu.Core.Editor
{
    // Preserve the existing test entry point for tools and menus after the narrative rework.
    public static class Chapter01PlayModeCheck
    {
        public static bool Running => Chapter01ReworkPlayCheck.Running;
        public static void Start() => Chapter01ReworkPlayCheck.Start();
    }
}
