return new {
 playing=UnityEditor.EditorApplication.isPlaying,
 scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene().path,
 models=UnityEditor.AssetDatabase.FindAssets("t:Model",new[]{"Assets/_Game/Art/Characters/MaGiamSinh","Assets/_Game/Animations"}).Select(UnityEditor.AssetDatabase.GUIDToAssetPath).Where(p=>p.EndsWith(".fbx",System.StringComparison.OrdinalIgnoreCase)).Select(p=>new{path=p,rig=((UnityEditor.ModelImporter)UnityEditor.AssetImporter.GetAtPath(p)).animationType.ToString(),avatars=UnityEditor.AssetDatabase.LoadAllAssetsAtPath(p).OfType<UnityEngine.Avatar>().Select(a=>new{a.name,a.isHuman,a.isValid}).ToArray(),clips=UnityEditor.AssetDatabase.LoadAllAssetsAtPath(p).OfType<UnityEngine.AnimationClip>().Where(c=>!c.name.StartsWith("__preview__")).Select(c=>new{c.name,c.length,c.isHumanMotion,c.isLooping}).ToArray()}).ToArray()
};
