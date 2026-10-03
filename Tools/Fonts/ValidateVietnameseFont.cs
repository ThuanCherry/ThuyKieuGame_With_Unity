ThuyKieu.UI.Editor.VietnameseFontSetup.Validate();
var font=UnityEditor.AssetDatabase.LoadAssetAtPath<TMPro.TMP_FontAsset>(ThuyKieu.UI.Editor.VietnameseFontSetup.FontPath);
return new {font.name,mode=font.atlasPopulationMode.ToString(),characters=font.characterTable.Count,
    dependencies=UnityEditor.AssetDatabase.GetDependencies(ThuyKieu.UI.Editor.VietnameseFontSetup.FontPath),
    subAssets=UnityEditor.AssetDatabase.LoadAllAssetsAtPath(ThuyKieu.UI.Editor.VietnameseFontSetup.FontPath).Select(a=>new{a.name,type=a.GetType().Name}).ToArray()};
