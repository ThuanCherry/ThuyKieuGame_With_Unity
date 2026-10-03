var source=UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Font>("Assets/TextMesh Pro/Fonts/LiberationSans.ttf");
UnityEngine.TextCore.LowLevel.FontEngine.InitializeFontEngine();
var result=UnityEngine.TextCore.LowLevel.FontEngine.LoadFontFace(source,64);
var required=Enumerable.Range(0x1EA0,0x1EFA-0x1EA0).Concat("ĂăĐđĨĩŨũƠơƯư\u0300\u0301\u0303\u0309\u0323\u0306\u0302\u031B".Select(c=>(int)c));
return new {source=source.name,result,missing=required.Where(c=>!UnityEngine.TextCore.LowLevel.FontEngine.TryGetGlyphIndex((uint)c,out var index)||index==0).Select(c=>"U+"+c.ToString("X4")).ToArray()};
