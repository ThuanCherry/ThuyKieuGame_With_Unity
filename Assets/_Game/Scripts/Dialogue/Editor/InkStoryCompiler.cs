using System;
using System.IO;
using System.Text;
using UnityEditor;

namespace ThuyKieu.Dialogue.Editor
{
    public class InkStoryCompiler : AssetPostprocessor, Ink.IFileHandler
    {
        public const string Source = "Assets/_Game/Data/Dialogue/Ink/Main.ink";
        public const string Output = "Assets/_Game/Data/Dialogue/Ink/Main.json";
        public string ResolveInkFilename(string name) => Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Source), name));
        public string LoadInkFileContents(string path) => File.ReadAllText(path, Encoding.UTF8);

        [MenuItem("ThuyKieu/Dialogue/Compile Main Ink")]
        public static void Compile()
        {
            bool failed = false;
            var compiler = new Ink.Compiler(File.ReadAllText(Source, Encoding.UTF8), new Ink.Compiler.Options
            {
                sourceFilename = Source,
                fileHandler = new InkStoryCompiler(),
                errorHandler = (message, type) =>
                {
                    if (type == Ink.ErrorType.Error) { failed = true; UnityEngine.Debug.LogError(message); }
                    else UnityEngine.Debug.LogWarning(message);
                }
            });
            var story = compiler.Compile();
            if (failed || story == null) throw new InvalidOperationException("Ink compilation failed; Main.json was preserved.");
            File.WriteAllText(Output, story.ToJson(), new UTF8Encoding(false));
            AssetDatabase.ImportAsset(Output);
        }

        private static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved, string[] movedFrom)
        {
            foreach (string path in imported)
                if (path.StartsWith("Assets/_Game/Data/Dialogue/Ink/") && path.EndsWith(".ink"))
                { EditorApplication.delayCall += Compile; break; }
        }
    }
}
