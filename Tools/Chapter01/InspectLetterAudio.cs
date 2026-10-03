var audio=UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Environment.Chapter01Audio>();
var serialized=new UnityEditor.SerializedObject(audio);
var clip=serialized.FindProperty("_dialogueLetter").objectReferenceValue as UnityEngine.AudioClip;
var sources=audio.GetComponentsInChildren<UnityEngine.AudioSource>();
return new {playing=UnityEditor.EditorApplication.isPlaying,scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene().path,
    clip=clip==null?null:new{clip.name,clip.length,clip.loadState},
    listenerVolume=UnityEngine.AudioListener.volume,listenerPaused=UnityEngine.AudioListener.pause,
    sources=sources.Select(s=>new{s.name,s.volume,s.mute,s.spatialBlend,s.isPlaying}).ToArray()};
