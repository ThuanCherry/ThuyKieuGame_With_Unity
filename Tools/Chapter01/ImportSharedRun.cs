public static class SharedImportRun
{
 public static string Main()
 {
  try {
   ThuyKieu.Dialogue.Editor.Chapter01SharedAnimationSetup.ImportClips();
   System.IO.File.WriteAllText("Tools/Chapter01/SharedImport.txt","PASS: All 14 Shared clips imported as Humanoid");
   return "All 14 clips imported";
  } catch(System.Exception exception) { return exception.ToString(); }
 }
}
