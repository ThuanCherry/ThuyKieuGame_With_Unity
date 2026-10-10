var t = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameViewSize");
return t.GetProperties(System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic).Select(p=>new {p.Name,p.CanWrite,type=p.PropertyType.ToString()}).ToArray();
