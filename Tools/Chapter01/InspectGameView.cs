var asm = typeof(UnityEditor.Editor).Assembly;
var type = asm.GetType("UnityEditor.GameView");
return new {properties=type.GetProperties(System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic).Where(p=>p.Name.Contains("Size")||p.Name.Contains("Resolution")).Select(p=>p.Name).ToArray(), methods=asm.GetType("UnityEditor.GameViewSizes").GetMethods().Select(m=>m.ToString()).ToArray()};
