var camera = Camera.main;
var oldPosition=camera.transform.position; var oldRotation=camera.transform.rotation;
camera.transform.position=new Vector3(3,3,0); camera.transform.LookAt(new Vector3(0,.5f,3.1f));
var target=new RenderTexture(1200,900,24); var texture=new Texture2D(1200,900,TextureFormat.RGB24,false); var oldTarget=camera.targetTexture; var oldActive=RenderTexture.active;
try { camera.targetTexture=target; camera.Render(); RenderTexture.active=target; texture.ReadPixels(new Rect(0,0,1200,900),0,0); texture.Apply(); System.IO.File.WriteAllBytes("Tools/Chapter01/Seats.png",texture.EncodeToPNG()); }
finally {camera.targetTexture=oldTarget; RenderTexture.active=oldActive; camera.transform.SetPositionAndRotation(oldPosition,oldRotation); target.Release(); UnityEngine.Object.DestroyImmediate(target); UnityEngine.Object.DestroyImmediate(texture);}
return "captured";
