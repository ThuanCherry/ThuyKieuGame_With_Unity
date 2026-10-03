var assembly=typeof(UnityEditor.EditorApplication).Assembly;
var entries=assembly.GetType("UnityEditor.LogEntries");
var entryType=assembly.GetType("UnityEditor.LogEntry");
var flags=System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic;
var lines=new System.Collections.Generic.List<string>();
entries.GetMethod("StartGettingEntries",flags).Invoke(null,null);
try
{
    int count=(int)entries.GetMethod("GetCount",flags).Invoke(null,null);
    for(int i=Math.Max(0,count-10);i<count;i++)
    {
        var entry=Activator.CreateInstance(entryType);
        entries.GetMethod("GetEntryInternal",flags).Invoke(null,new[]{(object)i,entry});
        lines.Add((string)entryType.GetField("message",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic).GetValue(entry));
    }
    return lines;
}
finally {entries.GetMethod("EndGettingEntries",flags).Invoke(null,null);}
