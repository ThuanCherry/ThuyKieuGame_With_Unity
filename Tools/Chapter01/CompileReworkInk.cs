ThuyKieu.Dialogue.Editor.InkStoryCompiler.Compile();
var story = new Ink.Runtime.Story(UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.TextAsset>(ThuyKieu.Dialogue.Editor.InkStoryCompiler.Output).text);
var lines = new System.Collections.Generic.List<object>();
for (int i = 0; i < 15 && story.canContinue; i++) { var line = story.Continue(); lines.Add(new {line, tags = story.currentTags.ToArray()}); }
return Newtonsoft.Json.JsonConvert.SerializeObject(lines);
