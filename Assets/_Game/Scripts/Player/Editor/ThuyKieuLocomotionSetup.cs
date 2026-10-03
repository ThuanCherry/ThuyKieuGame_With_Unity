using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ThuyKieu.Player.Editor
{
    public static class ThuyKieuLocomotionSetup
    {
        public const string ControllerPath = "Assets/_Game/Animations/ThuyKieu/ThuyKieuLocomotion.overrideController";

        public static AnimatorOverrideController GetController(RuntimeAnimatorController shared)
        {
            var walk = Clip("TK-Walking");
            var run = Clip("TK-Running");
            var controller = AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(ControllerPath);
            if (controller == null)
            {
                controller = new AnimatorOverrideController(shared) { name = "ThuyKieuLocomotion" };
                AssetDatabase.CreateAsset(controller, ControllerPath);
            }
            else controller.runtimeAnimatorController = shared;
            controller["Walking"] = walk;
            controller["Running"] = run;
            EditorUtility.SetDirty(controller);
            return controller;
        }

        public static void ConfigureMovement(PlayerMovement movement)
        {
            var serialized = new SerializedObject(movement);
            // Shorter travel per stride fits Kiều's own clips; easing softens starts and stops.
            serialized.FindProperty("walkSpeed").floatValue = 2.1f;
            serialized.FindProperty("runSpeed").floatValue = 4.8f;
            serialized.FindProperty("rotationSpeed").floatValue = 540f;
            serialized.FindProperty("_animationSmoothTime").floatValue = .13f;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            if (PrefabUtility.IsPartOfPrefabInstance(movement)) PrefabUtility.RecordPrefabInstancePropertyModifications(movement);
            EditorUtility.SetDirty(movement);
        }

        private static AnimationClip Clip(string name)
        {
            var clip = AssetDatabase.LoadAllAssetsAtPath("Assets/_Game/Animations/ThuyKieu/" + name + ".fbx")
                .OfType<AnimationClip>().FirstOrDefault(c => !c.name.StartsWith("__preview__"));
            if (clip == null || !clip.isHumanMotion || !clip.isLooping)
                throw new InvalidOperationException(name + " must be a looping Humanoid clip.");
            return clip;
        }
    }
}
