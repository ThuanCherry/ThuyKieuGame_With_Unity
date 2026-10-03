if(UnityEditor.EditorApplication.isPlaying) throw new System.InvalidOperationException("Stop Play Mode before saving appearance.");
string texturePath="Assets/_Game/Art/Characters/ThuyKieu/Tectures/texture_pbr_20250901.png";
string materialPath="Assets/_Game/Art/Characters/ThuyKieu/Materials/ThuyKieu_URP.mat";
var texture=UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(texturePath);
if(texture==null) throw new System.InvalidOperationException("Missing base colour texture.");
var material=UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(materialPath);
if(material==null){material=new UnityEngine.Material(UnityEngine.Shader.Find("Universal Render Pipeline/Lit")); UnityEditor.AssetDatabase.CreateAsset(material,materialPath);}
material.SetTexture("_BaseMap",texture);
material.SetColor("_BaseColor",UnityEngine.Color.white);
material.SetFloat("_Smoothness",0.2f);
material.SetFloat("_Metallic",0f);
string normalPath="Assets/_Game/Art/Characters/ThuyKieu/Tectures/texture_pbr_20250901_normal.png";
var importer=(UnityEditor.TextureImporter)UnityEditor.AssetImporter.GetAtPath(normalPath);
if(importer.textureType!=UnityEditor.TextureImporterType.NormalMap){importer.textureType=UnityEditor.TextureImporterType.NormalMap; importer.SaveAndReimport();}
material.SetTexture("_BumpMap",UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(normalPath));
material.EnableKeyword("_NORMALMAP");
UnityEditor.EditorUtility.SetDirty(material);
var player=UnityEngine.GameObject.Find("ThuyKieu");
foreach(var renderer in player.GetComponentsInChildren<UnityEngine.SkinnedMeshRenderer>(true)) {
 renderer.sharedMaterials=Enumerable.Repeat(material,renderer.sharedMaterials.Length).ToArray();
 UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(renderer);
}
UnityEditor.PrefabUtility.ApplyPrefabInstance(player,UnityEditor.InteractionMode.AutomatedAction);
UnityEditor.AssetDatabase.SaveAssets();
UnityEditor.SceneManagement.EditorSceneManager.SaveScene(player.scene);
return new{materialPath,texture=texture.name,uvs=player.GetComponentInChildren<UnityEngine.SkinnedMeshRenderer>().sharedMesh.uv.Length};
