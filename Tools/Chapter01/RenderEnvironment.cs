using UnityEngine;
using System.IO;
public static class RenderEnvironment
{
    public static string Main()
    {
        var camera=Camera.main;
        Vector3 position=camera.transform.position; Quaternion rotation=camera.transform.rotation;
        var oldTarget=camera.targetTexture; var active=RenderTexture.active;
        var target=new RenderTexture(1600,900,24); var texture=new Texture2D(1600,900,TextureFormat.RGB24,false);
        Vector3[] positions={new Vector3(7.8f,4.6f,-11.8f),new Vector3(.8f,2.45f,-1.45f),new Vector3(1.4f,2.4f,1.4f)};
        Vector3[] looks={new Vector3(0,2,0),new Vector3(0,1.35f,3.2f),new Vector3(0,.85f,3.1f)};
        string[] names={"Exterior","Interior","Contract"};
        try
        {
            for(int i=0;i<names.Length;i++)
            {
                camera.transform.position=positions[i];camera.transform.LookAt(looks[i]);camera.targetTexture=target;
                camera.Render();RenderTexture.active=target;texture.ReadPixels(new Rect(0,0,1600,900),0,0);texture.Apply();
                File.WriteAllBytes("Tools/Chapter01/Environment"+names[i]+".png",texture.EncodeToPNG());
            }
        }
        finally
        {
            camera.targetTexture=oldTarget;camera.transform.SetPositionAndRotation(position,rotation);RenderTexture.active=active;
            target.Release();Object.DestroyImmediate(target);Object.DestroyImmediate(texture);
        }
        return "Three environment views captured";
    }
}
