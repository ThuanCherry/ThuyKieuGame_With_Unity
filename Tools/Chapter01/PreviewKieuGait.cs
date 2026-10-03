using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public static class PreviewKieuGait
{
    public static string Main()
    {
        string[] paths={"Assets/_Game/Animations/Shared/Walking.fbx","Assets/_Game/Animations/Shared/Running.fbx","Assets/_Game/Animations/ThuyKieu/TK-Walking.fbx","Assets/_Game/Animations/ThuyKieu/TK-Running.fbx"};
        foreach(string path in paths)
        {
            var clip=AssetDatabase.LoadAllAssetsAtPath(path).OfType<AnimationClip>().First(c=>!c.name.StartsWith("__preview__"));
            var preview=new PreviewRenderUtility();
            var go=Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Game/Prefabs/Player/ThuyKieu.prefab"));
            go.transform.SetPositionAndRotation(Vector3.zero,Quaternion.identity);preview.AddSingleGO(go);
            var animator=go.GetComponentInChildren<Animator>();animator.runtimeAnimatorController=null;animator.applyRootMotion=false;animator.cullingMode=AnimatorCullingMode.AlwaysAnimate;
            var graph=PlayableGraph.Create("GaitPreview");graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
            var playable=AnimationClipPlayable.Create(graph,clip);playable.SetApplyFootIK(true);
            var output=AnimationPlayableOutput.Create(graph,"Pose",animator);output.SetSourcePlayable(playable);graph.Play();
            var sheet=new Texture2D(1600,600,TextureFormat.RGB24,false);
            try
            {
                for(int i=0;i<4;i++)
                {
                    playable.SetTime(clip.length*i*.25);graph.Evaluate(0);
                    preview.BeginStaticPreview(new Rect(0,0,400,600));
                    preview.camera.fieldOfView=32;preview.camera.transform.position=new Vector3(1.7f,1.2f,3.9f);preview.camera.transform.LookAt(new Vector3(0,.9f,0));
                    preview.camera.nearClipPlane=.01f;preview.camera.farClipPlane=30;preview.camera.clearFlags=CameraClearFlags.SolidColor;preview.camera.backgroundColor=new Color(.13f,.16f,.2f);
                    preview.lights[0].intensity=1.5f;preview.lights[0].transform.rotation=Quaternion.Euler(35,200,0);preview.lights[1].intensity=.7f;
                    preview.Render(true);var frame=preview.EndStaticPreview();sheet.SetPixels(i*400,0,400,600,frame.GetPixels());Object.DestroyImmediate(frame);
                }
                sheet.Apply();File.WriteAllBytes("Tools/Chapter01/Gait_"+clip.name+".png",sheet.EncodeToPNG());
            }
            finally{graph.Destroy();Object.DestroyImmediate(sheet);preview.Cleanup();}
        }
        return "Four gait contact sheets saved";
    }
}
