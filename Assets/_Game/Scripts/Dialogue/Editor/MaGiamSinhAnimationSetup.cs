using System;
using System.IO;
using System.Linq;
using ThuyKieu.Core;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ThuyKieu.Dialogue.Editor
{
    public static class MaGiamSinhAnimationSetup
    {
        public const string ModelPath = "Assets/_Game/Art/Characters/MaGiamSinh/Models/MaGiamSinh_Rigged.fbx";
        public const string ControllerPath = "Assets/_Game/Animations/MaGiamSinh/MaGiamSinh.controller";

        public static AnimatorController GetController()
        {
            var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath);
            if (controller != null) return controller;
            Directory.CreateDirectory(Path.GetDirectoryName(ControllerPath));
            AssetDatabase.Refresh();
            controller = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
            controller.AddParameter("DialogueTalking", AnimatorControllerParameterType.Bool);
            var machine = controller.layers[0].stateMachine;
            var idle = machine.AddState("Idle");
            var talking = machine.AddState("Talking");
            idle.motion = Clip("TK-Idle");
            talking.motion = Clip("TK-Talking");
            machine.defaultState = idle;
            var enter = idle.AddTransition(talking);
            enter.hasExitTime = false; enter.duration = 0.18f;
            enter.AddCondition(AnimatorConditionMode.If, 0, "DialogueTalking");
            var leave = talking.AddTransition(idle);
            leave.hasExitTime = false; leave.duration = 0.18f;
            leave.AddCondition(AnimatorConditionMode.IfNot, 0, "DialogueTalking");
            EditorUtility.SetDirty(controller);
            return controller;
        }

        private static AnimationClip Clip(string name) => AssetDatabase.LoadAllAssetsAtPath(
            "Assets/_Game/Animations/ThuyKieu/" + name + ".fbx").OfType<AnimationClip>()
            .First(c => !c.name.StartsWith("__preview__"));

        [MenuItem("ThuyKieu/Chapter 1/Configure Ma Giam Sinh animation")]
        public static void Configure()
        {
            if (Application.isPlaying) throw new InvalidOperationException("Stop Play Mode before setup.");
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (scene.path != "Assets/_Game/Scenes/Chapter01_GiaBien.unity")
                throw new InvalidOperationException("Open Chapter01_GiaBien first.");
            var importer = (ModelImporter)AssetImporter.GetAtPath(ModelPath);
            importer.animationType = ModelImporterAnimationType.Human;
            importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
            importer.importNormals = ModelImporterNormals.Calculate;
            importer.SaveAndReimport();
            var avatar = AssetDatabase.LoadAllAssetsAtPath(ModelPath).OfType<Avatar>().FirstOrDefault();
            if (avatar == null || !avatar.isHuman || !avatar.isValid)
                throw new InvalidOperationException("Mã Giám Sinh has no valid Humanoid avatar.");
            var director = UnityEngine.Object.FindAnyObjectByType<Chapter01Director>();
            var serialized = new SerializedObject(director);
            var root = (GameObject)serialized.FindProperty("_maGiamSinh").objectReferenceValue;
            var modelTransform = root.transform.Find("RiggedModel");
            GameObject model;
            if (modelTransform == null)
            {
                model = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(ModelPath));
                model.name = "RiggedModel";
                model.transform.SetParent(root.transform, false);
                var renderers = model.GetComponentsInChildren<Renderer>();
                Bounds bounds = renderers[0].bounds;
                foreach (var renderer in renderers) bounds.Encapsulate(renderer.bounds);
                float scale = 1.78f / bounds.size.y;
                model.transform.localScale *= scale;
                // Bounds are in world space because the root already has scene placement/rotation.
                // Transform the vertical floor correction independently of horizontal placement.
                model.transform.localPosition = Vector3.up * (root.transform.position.y - bounds.min.y) * scale;
                var material = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Game/Materials/MaGiamSinhChapter01.mat");
                if (material != null)
                    foreach (var renderer in renderers) renderer.sharedMaterials = Enumerable.Repeat(material, renderer.sharedMaterials.Length).ToArray();
            }
            else model = modelTransform.gameObject;
            foreach (Transform child in root.transform)
                if (child.gameObject != model && child.GetComponent<TMPro.TMP_Text>() == null
                    && child.GetComponentsInChildren<Renderer>(true).Length > 0) child.gameObject.SetActive(false);
            model.SetActive(true);
            root.name = "MaGiamSinh";
            var animator = model.GetComponent<Animator>() ?? model.AddComponent<Animator>();
            animator.avatar = avatar;
            animator.runtimeAnimatorController = GetController();
            animator.applyRootMotion = false;
            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            serialized.FindProperty("_maAnimator").objectReferenceValue = animator;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
        }
    }
}
