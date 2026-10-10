using System;
using System.Linq;
using ThuyKieu.Interaction;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThuyKieu.Core.Editor
{
    public static class Chapter01PresentationFix
    {
        public static void Apply()
        {
            var scene = SceneManager.GetActiveScene();
            if (Application.isPlaying || scene.path != Chapter01Setup.ScenePath)
                throw new InvalidOperationException("Open Chapter 1 in Edit Mode.");
            var mother = GameObject.Find("MeKieu").GetComponentInChildren<Animator>();
            ConfigureSad(mother);
            var presets = GameObject.Find("ReworkCameraPresets").transform;
            Shot(presets.Find("Mother"), new Vector3(.05f, 1.45f, 2.2f), mother.transform.root.position + Vector3.up * 1.05f);
            Shot(presets.Find("HallTwoShot"), new Vector3(.1f, 1.7f, 1.1f), new Vector3(1.15f, 1f, 3.1f));
            var house = GameObject.Find("VuongGia/MainHouse").transform;
            var room = house.Find("KieuRoom");
            var door = room.Find("KieuRoomDoor");
            door.position = new Vector3(-4.295f, 0, 2.5f);
            var leaf = door.Find("Leaf"); leaf.localRotation = Quaternion.identity;
            var panel = leaf.Find("Panel");
            panel.localPosition = new Vector3(.595f, 1.2975f, 0);
            panel.localScale = new Vector3(1.19f, 2.585f, .12f);
            foreach (var frame in room.GetComponentsInChildren<Transform>())
                if (frame.name.StartsWith("DoorFrame"))
                {
                    var position = frame.position; position.z = 2.5f; frame.position = position;
                    frame.localScale = new Vector3(.10f, 2.60f, .24f);
                }
            var main = house.Find("MainDoor");
            for (int sign = -1; sign <= 1; sign += 2)
            {
                var pivot = main.Find(sign < 0 ? "LeftPivot" : "RightPivot");
                pivot.localPosition = new Vector3(sign * 1.6f, 0, 0);
                pivot.localRotation = Quaternion.identity;
                var model = pivot.Find("DoorLeaf");
                model.localScale = new Vector3(1.54f, 1.29f, 1.3f);
                var bounds = model.GetComponentInChildren<Renderer>().bounds;
                var desired = main.position + new Vector3(sign * .80f, bounds.center.y, 0);
                model.position += desired - bounds.center;
            }
            var serialized = new SerializedObject(main.GetComponent<ProximityDoor>());
            serialized.FindProperty("_ajarAngle").floatValue = 0;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            Physics.SyncTransforms();
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
        }

        private static void Shot(Transform preset, Vector3 position, Vector3 target)
        {
            preset.position = position;
            preset.LookAt(target);
        }

        private static void ConfigureSad(Animator animator)
        {
            const string path = "Assets/_Game/Animations/Shared/Sitting Sad.fbx";
            var importer = (ModelImporter)AssetImporter.GetAtPath(path);
            importer.animationType = ModelImporterAnimationType.Human;
            importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
            importer.importAnimation = true;
            var clips = importer.clipAnimations.Length > 0 ? importer.clipAnimations : importer.defaultClipAnimations;
            foreach (var settings in clips)
            {
                settings.name = "Sitting Sad";
                settings.loopTime = true; settings.loopPose = true;
                settings.lockRootRotation = true; settings.lockRootPositionXZ = true; settings.lockRootHeightY = true;
                settings.keepOriginalOrientation = true; settings.keepOriginalPositionXZ = true; settings.heightFromFeet = true;
            }
            importer.clipAnimations = clips;
            importer.SaveAndReimport();
            var clip = AssetDatabase.LoadAllAssetsAtPath(path).OfType<AnimationClip>().First(c => !c.name.StartsWith("__preview"));
            if (!clip.isHumanMotion) throw new InvalidOperationException("Sitting Sad must be Humanoid.");
            RuntimeAnimatorController runtime = animator.runtimeAnimatorController;
            if (runtime is AnimatorOverrideController overrides) runtime = overrides.runtimeAnimatorController;
            var controller = (AnimatorController)runtime;
            var machine = controller.layers[0].stateMachine;
            var sad = machine.states.Select(s => s.state).FirstOrDefault(s => s.name == "SittingSad");
            if (sad == null)
            {
                sad = machine.AddState("SittingSad");
                var enter = machine.AddAnyStateTransition(sad);
                enter.hasExitTime = false; enter.duration = .2f; enter.canTransitionToSelf = false;
                enter.AddCondition(AnimatorConditionMode.If, 0, "IsSitting");
                enter.AddCondition(AnimatorConditionMode.Equals, 1, "DialogueEmotion");
                var seated = machine.states.First(s => s.state.name == "SittingIdle").state;
                var leave = sad.AddTransition(seated);
                leave.hasExitTime = false; leave.duration = .2f;
                leave.AddCondition(AnimatorConditionMode.NotEqual, 1, "DialogueEmotion");
            }
            sad.motion = clip;
            EditorUtility.SetDirty(controller);
        }
    }
}
