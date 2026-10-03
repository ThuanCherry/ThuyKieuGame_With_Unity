using UnityEngine;
using UnityEditor;
using System.IO;
public static class PreviewEnvironmentAssets
{
    public static string Main()
    {
        foreach (string name in new[]{"Architecture/KieuHouse_Exterior", "Props/TableSet", "Props/Altar"})
        {
            var preview = new PreviewRenderUtility();
            var go = (GameObject)Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Game/Prefabs/" + name + ".prefab"));
            preview.AddSingleGO(go);
            var rs = go.GetComponentsInChildren<Renderer>();
            var b = rs[0].bounds; foreach(var r in rs) b.Encapsulate(r.bounds);
            preview.camera.transform.position = b.center + new Vector3(0.5f,0.45f,-1).normalized * b.size.magnitude * 1.2f;
            preview.camera.transform.LookAt(b.center);
            preview.camera.nearClipPlane = 0.01f; preview.camera.farClipPlane = 100;
            preview.camera.backgroundColor = new Color(.15f,.19f,.24f);
            preview.camera.clearFlags = CameraClearFlags.SolidColor;
            preview.lights[0].intensity = 1.5f; preview.lights[0].transform.rotation = Quaternion.Euler(40,-30,0);
            preview.lights[1].intensity = .7f;
            preview.BeginStaticPreview(new Rect(0,0,900,650));
            preview.camera.fieldOfView = 45;
            preview.camera.transform.position = b.center + new Vector3(0.5f,0.35f,-1).normalized * b.size.magnitude * 1.4f;
            preview.camera.transform.LookAt(b.center);
            preview.Render(true);
            var texture = preview.EndStaticPreview();
            File.WriteAllBytes("Tools/Chapter01/" + Path.GetFileName(name) + "Preview.png",texture.EncodeToPNG());
            Object.DestroyImmediate(texture); preview.Cleanup();
        }
        return "Three asset previews saved";
    }
}
