var camera = Camera.main;
var target = new RenderTexture(1600, 900, 24);
var texture = new Texture2D(1600, 900, TextureFormat.RGB24, false);
var oldTarget = camera.targetTexture;
var oldActive = RenderTexture.active;
try {
 camera.targetTexture = target; camera.Render(); RenderTexture.active = target;
 texture.ReadPixels(new Rect(0, 0, 1600, 900), 0, 0); texture.Apply();
 System.IO.File.WriteAllBytes("Tools/Chapter01/KieuBedroom.png", texture.EncodeToPNG());
} finally {
 camera.targetTexture = oldTarget; RenderTexture.active = oldActive;
 target.Release(); UnityEngine.Object.DestroyImmediate(target); UnityEngine.Object.DestroyImmediate(texture);
}
var room = GameObject.Find("VuongGia/MainHouse/KieuRoom");
var colliders = room.GetComponentsInChildren<Collider>().Where(c => c.enabled && !c.isTrigger).Select(c => c.name).ToArray();
return new {image = "Tools/Chapter01/KieuBedroom.png", colliders};
