var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
var paths = AssetDatabase.FindAssets("t:Prefab", new[]{"Assets/_Game/Prefabs/Architecture", "Assets/_Game/Prefabs/Props", "Assets/_Game/Prefabs/Vegetation"}).Select(AssetDatabase.GUIDToAssetPath);
var report = new {
    scene = scene.path, dirty = scene.isDirty, playing = Application.isPlaying,
    prefabs = paths.Select(p => { var go = AssetDatabase.LoadAssetAtPath<GameObject>(p); var rs = go.GetComponentsInChildren<Renderer>(true); var b = rs[0].bounds; foreach (var r in rs) b.Encapsulate(r.bounds); return new {path=p, size=b.size.ToString(), center=b.center.ToString(), scale=go.transform.localScale.ToString(), meshes=go.GetComponentsInChildren<MeshFilter>(true).Select(m=>new{m.name,vertices=m.sharedMesh?.vertexCount}), materials=rs.SelectMany(r=>r.sharedMaterials).Distinct().Select(m=>new{name=m?.name,shader=m?.shader.name,texture=m?.mainTexture?.name,path=AssetDatabase.GetAssetPath(m?.mainTexture)}),colliders=go.GetComponentsInChildren<Collider>(true).Length};}).ToArray(),
    audio=AssetDatabase.FindAssets("t:AudioClip",new[]{"Assets"}).Select(AssetDatabase.GUIDToAssetPath).ToArray(),
    roots=scene.GetRootGameObjects().Select(go=>new{go.name,active=go.activeSelf,pos=go.transform.position.ToString(),scale=go.transform.localScale.ToString(),components=go.GetComponents<Component>().Select(c=>c?.GetType().Name)}).ToArray()
};
var json = Newtonsoft.Json.JsonConvert.SerializeObject(report, Newtonsoft.Json.Formatting.Indented); System.IO.File.WriteAllText("Tools/Chapter01/EnvironmentAssetAudit.json",json); return new { scene=scene.path, audioClips=report.audio.Length, prefabCount=report.prefabs.Length };
