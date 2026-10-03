var story = new Ink.Runtime.Story(UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.TextAsset>("Assets/_Game/Data/Dialogue/Ink/Main.json").text);
var lines = new System.Collections.Generic.List<object>();
for (int i=0; i<12 && story.canContinue; i++) { var text=story.Continue(); lines.Add(new{text,tags=story.currentTags.ToArray(),choices=story.currentChoices.Count}); }
return lines;
