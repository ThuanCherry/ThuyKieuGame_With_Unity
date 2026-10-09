var camera=Camera.main;
var target=new RenderTexture(1200,900,24); var texture=new Texture2D(1200,900,TextureFormat.RGB24,false); var oldTarget=camera.targetTexture; var oldActive=RenderTexture.active;
try { camera.targetTexture=target; camera.Render(); RenderTexture.active=target; texture.ReadPixels(new Rect(0,0,1200,900),0,0); texture.Apply(); System.IO.File.WriteAllBytes("Tools/Chapter01/MotherConversation.png",texture.EncodeToPNG()); }
finally {camera.targetTexture=oldTarget; RenderTexture.active=oldActive; target.Release(); UnityEngine.Object.DestroyImmediate(target); UnityEngine.Object.DestroyImmediate(texture);}
return camera.transform.position.ToString();
