using System;
using ThuyKieu.Dialogue;
using ThuyKieu.Player;
using UnityEditor;
using UnityEngine;

namespace ThuyKieu.Core.Editor
{
    public static class Chapter01CameraSetup
    {
        public static void Configure()
        {
            if (Application.isPlaying) throw new InvalidOperationException("Configure camera in Edit Mode.");
            var director = UnityEngine.Object.FindAnyObjectByType<Chapter01Director>();
            var camera = UnityEngine.Object.FindAnyObjectByType<ThirdPersonCameraController>();
            var player = UnityEngine.Object.FindAnyObjectByType<PlayerMovement>();
            var router = director.GetComponent<DialogueTagRouter>() ?? director.gameObject.AddComponent<DialogueTagRouter>();
            router.enabled = false;
            Transform root = GameObject.Find("DialogueCameraPresets")?.transform;
            if (root == null) root = new GameObject("DialogueCameraPresets").transform;
            string[] names = { "WS_VuongGia", "MCU_MeKieu", "CU_Kieu", "CU_Kieu_Eyes", "Reveal_MaGiamSinh", "POV_Kieu_MaHands", "Table_Contract", "Hero_Kieu_Rain" };
            Vector3[] positions = {
                new Vector3(0, 3, -5), new Vector3(-0.8f, 1.7f, 0), new Vector3(0.9f, 1.5f, 1.4f), new Vector3(0.6f, 1.6f, 1),
                new Vector3(1.2f, 1.6f, -1.3f), new Vector3(1.2f, 1.6f, -1.3f), new Vector3(1.3f, 2.2f, 1.8f), new Vector3(-1.2f, 1.8f, 3.2f)
            };
            Vector3[] targets = {
                new Vector3(0,1.2f,2),new Vector3(-2.5f,1.4f,1.5f),new Vector3(0,1.3f,0),new Vector3(0,1.5f,0),
                new Vector3(2.5f,1.4f,0.5f),new Vector3(2.5f,1.4f,0.5f),new Vector3(0,0.85f,3.1f),new Vector3(0,1.3f,0)
            };
            var serialized = new SerializedObject(router);
            serialized.FindProperty("_director").objectReferenceValue = director;
            serialized.FindProperty("_camera").objectReferenceValue = camera;
            var cues = serialized.FindProperty("_cameraCues"); cues.arraySize = names.Length;
            for (int i = 0; i < names.Length; i++)
            {
                bool playerRelative = i == 2 || i == 3 || i == 7;
                Transform parent = playerRelative ? player.transform : root;
                Transform preset = parent.Find(names[i]);
                if (preset == null) { preset = new GameObject(names[i]).transform; preset.SetParent(parent, false); }
                preset.localPosition = positions[i];
                preset.localRotation = Quaternion.LookRotation(targets[i] - positions[i]);
                var cue = cues.GetArrayElementAtIndex(i);
                cue.FindPropertyRelative("Name").stringValue = names[i];
                cue.FindPropertyRelative("Preset").objectReferenceValue = preset;
            }
            serialized.ApplyModifiedPropertiesWithoutUndo();
            router.enabled = true;
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
    }
}
