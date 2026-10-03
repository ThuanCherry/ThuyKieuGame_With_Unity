using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ThuyKieu.Player.Editor
{
    /// <summary>Explicit setup tools; never run automatically on import or replace existing assets.</summary>
    public static class ThuyKieuPlayableSetup
    {
        public const string ModelPath = "Assets/_Game/Art/Characters/ThuyKieu/Models/ThuyKieu_Rigged.fbx";
        public const string AnimationFolder = "Assets/_Game/Animations/ThuyKieu";
        public const string ControllerPath = AnimationFolder + "/ThuyKieu.controller";
        public const string PrefabPath = "Assets/_Game/Prefabs/Player/ThuyKieu.prefab";
        public const string ScenePath = "Assets/_Game/Scenes/ThuyKieu_Playground.unity";

        [MenuItem("ThuyKieu/Player/Configure existing Humanoid animations")]
        public static void ConfigureAnimations()
        {
            var avatar = AssetDatabase.LoadAllAssetsAtPath(ModelPath).OfType<Avatar>().FirstOrDefault();
            if (avatar == null || !avatar.isValid || !avatar.isHuman)
                throw new InvalidOperationException("ThuyKieu model must have a valid Humanoid avatar.");

            foreach (string path in AssetDatabase.FindAssets("t:Model", new[] { AnimationFolder })
                         .Select(AssetDatabase.GUIDToAssetPath).Where(p => p.EndsWith(".fbx", StringComparison.OrdinalIgnoreCase)))
            {
                var importer = (ModelImporter)AssetImporter.GetAtPath(path);
                importer.animationType = ModelImporterAnimationType.Human;
                importer.avatarSetup = ModelImporterAvatarSetup.CopyFromOther;
                importer.sourceAvatar = avatar;
                importer.importAnimation = true;
                var clips = importer.clipAnimations;
                if (clips.Length == 0) clips = importer.defaultClipAnimations;
                string name = Path.GetFileNameWithoutExtension(path);
                foreach (var clip in clips)
                {
                    clip.name = name;
                    clip.loopTime = !name.Contains("Picking Up") && !name.Contains("Look Around");
                    clip.loopPose = clip.loopTime;
                    // Keep body articulation while baking locomotion translation/rotation into pose.
                    clip.lockRootRotation = true;
                    clip.lockRootHeightY = true;
                    clip.lockRootPositionXZ = true;
                    clip.keepOriginalOrientation = true;
                    clip.keepOriginalPositionXZ = true;
                    clip.heightFromFeet = true;
                }
                importer.clipAnimations = clips;
                importer.SaveAndReimport();
                if (!GetClip(path).isHumanMotion)
                    throw new InvalidOperationException("Humanoid import failed: " + path);
            }
            AssetDatabase.SaveAssets();
        }

        [MenuItem("ThuyKieu/Player/Create playable prefab and test scene")]
        public static void CreatePlayable()
        {
            foreach (string path in new[] { ControllerPath, PrefabPath, ScenePath })
                if (File.Exists(path)) throw new InvalidOperationException("Asset already exists; inspect before editing: " + path);
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            Directory.CreateDirectory(Path.GetDirectoryName(PrefabPath));
            AssetDatabase.Refresh();

            var controller = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
            controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
            controller.AddParameter("IsCrouching", AnimatorControllerParameterType.Bool);
            controller.AddParameter("Talk", AnimatorControllerParameterType.Trigger);
            controller.AddParameter("Pickup", AnimatorControllerParameterType.Trigger);
            var machine = controller.layers[0].stateMachine;
            var locomotion = machine.AddState("Locomotion");
            machine.defaultState = locomotion;
            var tree = new BlendTree { name = "Idle Walk Run", blendType = BlendTreeType.Simple1D,
                blendParameter = "Speed", useAutomaticThresholds = false };
            AssetDatabase.AddObjectToAsset(tree, controller);
            tree.AddChild(GetClip(AnimationFolder + "/TK-Idle.fbx"), 0f);
            tree.AddChild(GetClip(AnimationFolder + "/TK-Walking.fbx"), 0.5f);
            tree.AddChild(GetClip(AnimationFolder + "/TK-Running.fbx"), 1f);
            locomotion.motion = tree;
            AddAction(machine, locomotion, "Talking", "TK-Talking", "Talk");
            AddAction(machine, locomotion, "Pickup", "TK-Picking Up Object", "Pickup");
            foreach (string name in new[] { "TK-Look Around", "TK-Sitting Idle", "TK-Sitting Talking" })
                machine.AddState(name.Substring(3)).motion = GetClip(AnimationFolder + "/" + name + ".fbx");

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var player = new GameObject("ThuyKieu") { tag = "Player" };
            var model = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(ModelPath));
            model.name = "Model";
            model.transform.SetParent(player.transform, false);
            var renderers = model.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (renderers.Length == 0) throw new InvalidOperationException("Rigged model has no skinned mesh.");
            var appearance = GetAppearanceMaterial();
            foreach (var renderer in renderers)
                renderer.sharedMaterials = Enumerable.Repeat(appearance, renderer.sharedMaterials.Length).ToArray();
            Bounds bounds = renderers[0].bounds;
            foreach (var renderer in renderers) bounds.Encapsulate(renderer.bounds);
            float scale = 1.65f / bounds.size.y;
            model.transform.localScale *= scale;
            // The Humanoid clips use feet-based baked root height. Bind-pose bounds would lift the animated feet.
            model.transform.localPosition = new Vector3(-bounds.center.x * scale, 0f, -bounds.center.z * scale);
            var animator = model.GetComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.avatar = AssetDatabase.LoadAllAssetsAtPath(ModelPath).OfType<Avatar>().First();
            animator.applyRootMotion = false;
            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            var capsule = player.AddComponent<CharacterController>();
            capsule.height = 1.65f;
            capsule.radius = 0.25f;
            capsule.center = Vector3.up * 0.825f;
            capsule.skinWidth = 0.025f;
            capsule.stepOffset = 0.25f;
            capsule.minMoveDistance = 0f;
            var movement = player.AddComponent<PlayerMovement>();
            var serialized = new SerializedObject(movement);
            serialized.FindProperty("_animator").objectReferenceValue = animator;
            serialized.FindProperty("_normalizeAnimatorSpeed").boolValue = true;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            var target = new GameObject("CameraTarget");
            target.transform.SetParent(player.transform, false);
            target.transform.localPosition = Vector3.up * 1.3f;
            PrefabUtility.SaveAsPrefabAssetAndConnect(player, PrefabPath, InteractionMode.AutomatedAction);

            var cameraObject = new GameObject("Main Camera") { tag = "MainCamera" };
            var camera = cameraObject.AddComponent<Camera>();
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 150f;
            cameraObject.AddComponent<AudioListener>();
            cameraObject.transform.rotation = Quaternion.Euler(12f, 0f, 0f);
            cameraObject.transform.position = target.transform.position - cameraObject.transform.forward * 4.5f;
            var follow = cameraObject.AddComponent<ThirdPersonCameraController>();
            var cameraSettings = new SerializedObject(follow);
            cameraSettings.FindProperty("target").objectReferenceValue = target.transform;
            cameraSettings.ApplyModifiedPropertiesWithoutUndo();
            var lightObject = new GameObject("Directional Light");
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 2f;
            light.shadows = LightShadows.Soft;
            lightObject.transform.rotation = Quaternion.Euler(45f, -30f, 0f);
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.55f, 0.55f, 0.6f);
            var groundMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit")) { name = "ThuyKieu Test Ground" };
            groundMaterial.color = new Color(0.28f, 0.35f, 0.28f);
            AssetDatabase.CreateAsset(groundMaterial, "Assets/_Game/Materials/Dev_PlayerMovement/ThuyKieuTestGround.mat");
            CreateBlock("Ground", new Vector3(0f, -0.25f, 0f), new Vector3(40f, 0.5f, 40f), groundMaterial);
            CreateBlock("CollisionTestWall", new Vector3(0f, 1f, 8f), new Vector3(8f, 2f, 0.5f), groundMaterial);
            CreateBlock("StepTest", new Vector3(4f, 0.075f, 3f), new Vector3(2f, 0.15f, 2f), groundMaterial);
            AssetDatabase.SaveAssets();
            EditorSceneManager.SaveScene(scene, ScenePath);
            Selection.activeGameObject = player;
        }

        private static AnimationClip GetClip(string path)
        {
            return AssetDatabase.LoadAllAssetsAtPath(path).OfType<AnimationClip>()
                .First(c => !c.name.StartsWith("__preview__", StringComparison.Ordinal));
        }

        private static Material GetAppearanceMaterial()
        {
            const string characterFolder = "Assets/_Game/Art/Characters/ThuyKieu";
            const string materialPath = characterFolder + "/Materials/ThuyKieu_URP.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            if (material != null) return material;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(characterFolder + "/Tectures/texture_pbr_20250901.png");
            if (texture == null) throw new InvalidOperationException("Missing ThuyKieu base colour texture.");
            Directory.CreateDirectory(characterFolder + "/Materials");
            AssetDatabase.Refresh();
            material = new Material(Shader.Find("Universal Render Pipeline/Lit")) { name = "ThuyKieu_URP" };
            material.SetTexture("_BaseMap", texture);
            material.SetColor("_BaseColor", Color.white);
            material.SetFloat("_Smoothness", 0.2f);
            AssetDatabase.CreateAsset(material, materialPath);
            return material;
        }

        private static void AddAction(AnimatorStateMachine machine, AnimatorState locomotion, string name, string clip, string trigger)
        {
            var action = machine.AddState(name);
            action.motion = GetClip(AnimationFolder + "/" + clip + ".fbx");
            var enter = locomotion.AddTransition(action);
            enter.hasExitTime = false;
            enter.duration = 0.15f;
            enter.AddCondition(AnimatorConditionMode.If, 0f, trigger);
            var exit = action.AddTransition(locomotion);
            exit.hasExitTime = true;
            exit.exitTime = 1f;
            exit.duration = 0.15f;
            var move = action.AddTransition(locomotion);
            move.hasExitTime = false;
            move.duration = 0.15f;
            move.AddCondition(AnimatorConditionMode.Greater, 0.05f, "Speed");
        }

        private static void CreateBlock(string name, Vector3 position, Vector3 size, Material material)
        {
            var block = GameObject.CreatePrimitive(PrimitiveType.Cube);
            block.name = name;
            block.transform.position = position;
            block.transform.localScale = size;
            block.GetComponent<Renderer>().sharedMaterial = material;
        }
    }
}
