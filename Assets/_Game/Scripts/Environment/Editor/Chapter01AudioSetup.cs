using System;
using System.Linq;
using ThuyKieu.Core.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThuyKieu.Environment.Editor
{
    public static class Chapter01AudioSetup
    {
        public const string AudioFolder = "Assets/_Game/Audio/Chapter01/";

        [MenuItem("ThuyKieu/Chapter 1/Assign audio")]
        public static void Apply()
        {
            var scene = SceneManager.GetActiveScene();
            if (Application.isPlaying || scene.path != Chapter01Setup.ScenePath)
                throw new InvalidOperationException("Open Chapter 1 in Edit Mode before assigning audio.");
            var audio = UnityEngine.Object.FindAnyObjectByType<Chapter01Audio>();
            if (audio == null) throw new InvalidOperationException("Chapter 1 environment audio is missing.");
            var sources = audio.GetComponentsInChildren<AudioSource>();
            string[] sourceNames = { "MusicSource", "RainSource", "WindSource" };
            string[] loopNames = { "SadStrings", "Rain", "Wind" };
            string[] fields = { "_paper", "_doorOpen", "_doorClose", "_uiConfirm", "_woodStep", "_stoneStep", "_dialogueLetter" };
            string[] clipNames = { "Paper", "DoorOpen", "DoorClose", "UIConfirm", "WoodStep", "StoneStep", "DialogueLetter" };
            // Validate everything before changing scene references.
            var loops = loopNames.Select(Load).ToArray();
            var clips = clipNames.Select(Load).ToArray();
            var loopSources = sourceNames.Select(name => sources.Single(source => source.name == name)).ToArray();
            Undo.RecordObjects(loopSources, "Assign Chapter 1 ambience");
            for (int i = 0; i < loops.Length; i++)
            {
                loopSources[i].clip = loops[i];
                loopSources[i].loop = true;
                EditorUtility.SetDirty(loopSources[i]);
            }
            var serialized = new SerializedObject(audio);
            for (int i = 0; i < fields.Length; i++)
                serialized.FindProperty(fields[i]).objectReferenceValue = clips[i];
            serialized.ApplyModifiedProperties();
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }

        private static AudioClip Load(string name)
        {
            var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(AudioFolder + name + ".wav");
            if (clip == null) throw new InvalidOperationException("Missing Chapter 1 clip: " + name);
            return clip;
        }
    }
}
