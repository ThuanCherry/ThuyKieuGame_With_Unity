UnityEditor.EditorApplication.delayCall += () => {
 try {
  ThuyKieu.Dialogue.Editor.Chapter01SharedAnimationSetup.ImportClips();
  System.IO.File.WriteAllText("Tools/Chapter01/SharedImport.txt","PASS: All 14 Shared clips imported as Humanoid");
 } catch(System.Exception exception) { System.IO.File.WriteAllText("Tools/Chapter01/SharedImport.txt",exception.ToString()); }
};
return "Shared import queued";
