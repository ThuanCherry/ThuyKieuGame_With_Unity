var scene=UnityEditor.SceneManagement.EditorSceneManager.NewPreviewScene();
var previous=UnityEngine.RenderTexture.active;
UnityEngine.RenderTexture target=null;
UnityEngine.Texture2D pixels=null;
UnityEngine.Camera previewCamera=null;
try
{
    var cameraObject=new UnityEngine.GameObject("FontPreviewCamera",typeof(UnityEngine.Camera));
    UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(cameraObject,scene);
    var camera=cameraObject.GetComponent<UnityEngine.Camera>();
    previewCamera=camera;
    camera.scene=scene;camera.cullingMask=1<<31;
    camera.transform.position=new UnityEngine.Vector3(0,0,-10);
    camera.orthographic=true;camera.orthographicSize=3.3f;
    camera.clearFlags=UnityEngine.CameraClearFlags.SolidColor;
    camera.backgroundColor=new UnityEngine.Color(.055f,.085f,.13f);
    var canvasObject=new UnityEngine.GameObject("FontPreviewCanvas",typeof(UnityEngine.RectTransform),typeof(UnityEngine.Canvas));
    UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(canvasObject,scene);
    canvasObject.GetComponent<UnityEngine.Canvas>().renderMode=UnityEngine.RenderMode.WorldSpace;
    canvasObject.layer=31;canvasObject.GetComponent<UnityEngine.Canvas>().worldCamera=camera;
    var rect=canvasObject.GetComponent<UnityEngine.RectTransform>();
    rect.sizeDelta=new UnityEngine.Vector2(1600,600);rect.localScale=UnityEngine.Vector3.one*.01f;
    var textObject=new UnityEngine.GameObject("VietnameseSample",typeof(UnityEngine.RectTransform),typeof(TMPro.TextMeshProUGUI));
    textObject.transform.SetParent(rect,false);
    textObject.layer=31;
    var text=textObject.GetComponent<TMPro.TextMeshProUGUI>();
    text.rectTransform.sizeDelta=new UnityEngine.Vector2(1450,550);
    text.font=UnityEditor.AssetDatabase.LoadAssetAtPath<TMPro.TMP_FontAsset>("Assets/Fonts/VietnameseTMP.asset");
    text.fontSize=38;text.color=UnityEngine.Color.white;
    text.text="TMP TIẾNG VIỆT — ATLAS STATIC\n\nThúy Kiều · Mẹ Kiều · Mã Giám Sinh\nCHƯƠNG 1: GIA BIẾN — Tiếp tục — Lựa chọn\n\nĂ Â Đ Ê Ô Ơ Ư / ă â đ ê ô ơ ư\nẮ Ằ Ẳ Ẵ Ặ · Ấ Ầ Ẩ Ẫ Ậ · Ế Ề Ể Ễ Ệ\nố ồ ổỗ ộ · ớ ờ ở ỡ ợ · ứ ừử ữ ự · ỳ ýỷ ỹ ỵ\n\n0123456789 · 100.000 ₫ · ← ↑ → ↓";
    text.ForceMeshUpdate();
    UnityEngine.Canvas.ForceUpdateCanvases();
    if(text.isTextOverflowing)throw new System.InvalidOperationException("Preview text overflow");
    target=new UnityEngine.RenderTexture(1600,660,24);
    camera.targetTexture=target;camera.Render();
    UnityEngine.RenderTexture.active=target;
    pixels=new UnityEngine.Texture2D(1600,660,UnityEngine.TextureFormat.RGB24,false);
    pixels.ReadPixels(new UnityEngine.Rect(0,0,1600,660),0,0);pixels.Apply();
    System.IO.File.WriteAllBytes("Tools/Fonts/VietnameseTMP-Preview.png",pixels.EncodeToPNG());
    return new{characters=text.textInfo.characterCount,staticAtlas=text.font.atlasPopulationMode.ToString(),preview="Tools/Fonts/VietnameseTMP-Preview.png"};
}
finally
{
    UnityEngine.RenderTexture.active=previous;
    if(previewCamera!=null)previewCamera.targetTexture=null;
    if(target!=null){target.Release();UnityEngine.Object.DestroyImmediate(target);}
    if(pixels!=null)UnityEngine.Object.DestroyImmediate(pixels);
    UnityEditor.SceneManagement.EditorSceneManager.ClosePreviewScene(scene);
}
