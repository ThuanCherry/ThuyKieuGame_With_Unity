if (UnityEditor.EditorApplication.isPlaying) return "Stop first";
foreach (var label in UnityEngine.Object.FindObjectsByType<TMPro.TextMeshPro>(UnityEngine.FindObjectsInactive.Include)) {
 if (label.GetComponent<ThuyKieu.UI.WorldLabelBillboard>()==null) label.gameObject.AddComponent<ThuyKieu.UI.WorldLabelBillboard>();
 label.fontSize=2;
 label.transform.rotation=UnityEngine.Quaternion.identity;
}
var rain=UnityEngine.GameObject.Find("CourtyardRain").GetComponent<UnityEngine.ParticleSystem>();
var main=rain.main; main.startSize=0.008f;
var renderer=rain.GetComponent<UnityEngine.ParticleSystemRenderer>(); renderer.lengthScale=1; renderer.velocityScale=0.008f;
foreach(var light in UnityEngine.Object.FindObjectsByType<UnityEngine.Light>()) if(light.type==UnityEngine.LightType.Point) light.shadows=UnityEngine.LightShadows.None;
UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
UnityEditor.SceneManagement.EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
return "Labels and rain updated";
