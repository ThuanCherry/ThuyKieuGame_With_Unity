using System;
using System.IO;
using System.Linq;
using ThuyKieu.Core;
using ThuyKieu.Player;
using ThuyKieu.Player.Editor;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ThuyKieu.Dialogue.Editor
{
    public static class Chapter01SharedAnimationSetup
    {
        public const string Folder = "Assets/_Game/Animations/Shared";
        public const string ControllerPath = Folder + "/Chapter01Shared.controller";
        private static readonly string[] Names = { "Idle", "Walking", "Running", "Talking_with_one_hand", "Crying", "Standing Arguing", "Agreeing", "Disagree", "Looking Around", "Picking Up Object", "Sitting Idle", "Sitting Talking", "Stand To Sit", "Sit To Stand" };

        public static void ImportClips()
        {
            foreach (string name in Names)
            {
                string path = Folder + "/" + name + ".fbx";
                var importer = (ModelImporter)AssetImporter.GetAtPath(path);
                if (importer == null) throw new InvalidOperationException("Missing clip: " + path);
                importer.animationType = ModelImporterAnimationType.Human;
                importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
                importer.importAnimation = true;
                var clips = importer.clipAnimations;
                if (clips.Length == 0) clips = importer.defaultClipAnimations;
                foreach (var clip in clips)
                {
                    clip.name = name;
                    clip.loopTime = name == "Idle" || name == "Walking" || name == "Running" || name == "Talking_with_one_hand"
                        || name == "Crying" || name == "Standing Arguing" || name == "Sitting Idle" || name == "Sitting Talking";
                    clip.loopPose = clip.loopTime;
                    clip.lockRootRotation = true; clip.lockRootHeightY = true; clip.lockRootPositionXZ = true;
                    clip.keepOriginalOrientation = true; clip.keepOriginalPositionXZ = true; clip.heightFromFeet = true;
                }
                importer.clipAnimations = clips;
                importer.SaveAndReimport();
                if (!Clip(name).isHumanMotion) throw new InvalidOperationException("Clip is not Humanoid: " + name);
            }
        }

        private static AnimationClip Clip(string name) => AssetDatabase.LoadAllAssetsAtPath(Folder + "/" + (name == "Talking" ? "Talking_with_one_hand" : name) + ".fbx")
            .OfType<AnimationClip>().First(c => !c.name.StartsWith("__preview__"));

        public static AnimatorController GetController()
        {
            var existing = AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath);
            if (existing != null)
            {
                foreach (var child in existing.layers[0].stateMachine.states)
                {
                    var state = child.state;
                    if (state.name == "Idle" && state.motion is BlendTree blend)
                    {
                        var children = blend.children;
                        for (int i = 0; i < children.Length; i++) children[i].motion = Clip(Names[i]);
                        blend.children = children;
                    }
                    else
                    {
                        string clip = state.name == "Arguing" ? "Standing Arguing" : state.name == "LookingAround" ? "Looking Around"
                            : state.name == "Pickup" ? "Picking Up Object" : state.name == "SittingIdle" ? "Sitting Idle"
                            : state.name == "SittingTalking" ? "Sitting Talking" : state.name == "StandToSit" ? "Stand To Sit"
                            : state.name == "SitToStand" ? "Sit To Stand" : state.name;
                        if (Names.Contains(clip) || clip == "Talking") state.motion = Clip(clip);
                    }
                }
                foreach (var transition in existing.layers[0].stateMachine.anyStateTransitions)
                { transition.hasFixedDuration = true; transition.duration = 0.15f; }
                foreach (var child in existing.layers[0].stateMachine.states)
                    foreach (var transition in child.state.transitions)
                    { transition.hasFixedDuration = true; transition.duration = Mathf.Min(transition.duration, 0.15f); }
                EditorUtility.SetDirty(existing);
                ConfigureSeatedActing(existing);
                ConfigureContractActing(existing);
                return existing;
            }
            var controller = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
            controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
            controller.AddParameter("DialogueTalking", AnimatorControllerParameterType.Bool);
            controller.AddParameter("DialogueEmotion", AnimatorControllerParameterType.Int);
            controller.AddParameter("IsSitting", AnimatorControllerParameterType.Bool);
            controller.AddParameter("Sit", AnimatorControllerParameterType.Trigger);
            controller.AddParameter("Stand", AnimatorControllerParameterType.Trigger);
            var machine = controller.layers[0].stateMachine;
            var idle = machine.AddState("Idle"); machine.defaultState = idle;
            var tree = new BlendTree { name = "Shared Idle Walk Run", blendType = BlendTreeType.Simple1D, blendParameter = "Speed", useAutomaticThresholds = false };
            AssetDatabase.AddObjectToAsset(tree, controller);
            tree.AddChild(Clip("Idle"), 0); tree.AddChild(Clip("Walking"), 0.5f); tree.AddChild(Clip("Running"), 1);
            idle.motion = tree;
            string[] stateNames = { "Talking", "Crying", "Arguing", "Agreeing", "Disagree", "LookingAround", "Pickup" };
            string[] clips = { "Talking", "Crying", "Standing Arguing", "Agreeing", "Disagree", "Looking Around", "Picking Up Object" };
            AnimatorState talking = null;
            for (int emotion = 0; emotion < stateNames.Length; emotion++)
            {
                var state = machine.AddState(stateNames[emotion]); state.motion = Clip(clips[emotion]);
                if (emotion == 0) talking = state;
                var enter = machine.AddAnyStateTransition(state);
                enter.hasExitTime = false; enter.duration = 0.15f; enter.canTransitionToSelf = false;
                enter.AddCondition(AnimatorConditionMode.If, 0, "DialogueTalking");
                enter.AddCondition(AnimatorConditionMode.Equals, emotion, "DialogueEmotion");
                enter.AddCondition(AnimatorConditionMode.IfNot, 0, "IsSitting");
                var stop = state.AddTransition(idle); stop.hasExitTime = false; stop.duration = 0.15f;
                stop.AddCondition(AnimatorConditionMode.IfNot, 0, "DialogueTalking");
                if (emotion >= 3)
                {
                    var finish = state.AddTransition(talking); finish.hasExitTime = true; finish.exitTime = 0.95f; finish.duration = 0.1f;
                    finish.AddCondition(AnimatorConditionMode.If, 0, "DialogueTalking");
                    state.AddStateMachineBehaviour<DialogueGestureReturn>().Configure(emotion);
                }
            }
            var sitDown = machine.AddState("StandToSit"); sitDown.motion = Clip("Stand To Sit");
            var seated = machine.AddState("SittingIdle"); seated.motion = Clip("Sitting Idle");
            var seatedTalk = machine.AddState("SittingTalking"); seatedTalk.motion = Clip("Sitting Talking");
            var standUp = machine.AddState("SitToStand"); standUp.motion = Clip("Sit To Stand");
            var sit = machine.AddAnyStateTransition(sitDown); sit.hasExitTime = false; sit.canTransitionToSelf = false;
            sit.AddCondition(AnimatorConditionMode.If, 0, "Sit"); sit.AddCondition(AnimatorConditionMode.If, 0, "IsSitting");
            var sitFinish = sitDown.AddTransition(seated); sitFinish.hasExitTime = true; sitFinish.exitTime = 0.95f;
            var speak = seated.AddTransition(seatedTalk); speak.hasExitTime = false; speak.AddCondition(AnimatorConditionMode.If, 0, "DialogueTalking");
            var silent = seatedTalk.AddTransition(seated); silent.hasExitTime = false; silent.AddCondition(AnimatorConditionMode.IfNot, 0, "DialogueTalking");
            var stand = machine.AddAnyStateTransition(standUp); stand.hasExitTime = false; stand.canTransitionToSelf = false;
            stand.AddCondition(AnimatorConditionMode.If, 0, "Stand"); stand.AddCondition(AnimatorConditionMode.If, 0, "IsSitting");
            var standFinish = standUp.AddTransition(idle); standFinish.hasExitTime = true; standFinish.exitTime = 0.95f;
            standUp.AddStateMachineBehaviour<DialogueGestureReturn>().Configure(0, true);
            EditorUtility.SetDirty(controller);
            ConfigureSeatedActing(controller);
            ConfigureContractActing(controller);
            return controller;
        }

        private static void ConfigureContractActing(AnimatorController controller)
        {
            var machine = controller.layers[0].stateMachine;
            var layers = controller.layers; layers[0].iKPass = true; controller.layers = layers;
            if (!controller.parameters.Any(p => p.name == "ContractActing"))
                controller.AddParameter("ContractActing", AnimatorControllerParameterType.Bool);
            // Authored actions must finish before ordinary dialogue can interrupt them.
            foreach (var transition in machine.anyStateTransitions)
                if (!transition.conditions.Any(c => c.parameter == "ContractActing"))
                    transition.AddCondition(AnimatorConditionMode.IfNot, 0, "ContractActing");
            foreach (string name in new[] { "Bowing", "Writing" })
            {
                var state = machine.states.Select(s => s.state).FirstOrDefault(s => s.name == name) ?? machine.AddState(name);
                state.motion = Clip(name == "Bowing" ? "Informal Bowing" : "Writing");
            }
            if (!controller.layers.Any(l => l.name == "Standing writing"))
            {
                var mask = new AvatarMask { name = "Standing writing right arm" };
                for (int i = 0; i < (int)AvatarMaskBodyPart.LastBodyPart; i++)
                    mask.SetHumanoidBodyPartActive((AvatarMaskBodyPart)i, false);
                mask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.RightArm, true);
                mask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.RightFingers, true);
                AssetDatabase.AddObjectToAsset(mask, controller);
                var upper = new AnimatorStateMachine { name = "Standing writing" };
                AssetDatabase.AddObjectToAsset(upper, controller);
                var writing = upper.AddState("Writing"); writing.motion = Clip("Writing"); upper.defaultState = writing;
                controller.AddLayer(new AnimatorControllerLayer { name = "Standing writing", stateMachine = upper,
                    avatarMask = mask, defaultWeight = 0, iKPass = true, blendingMode = AnimatorLayerBlendingMode.Override });
            }
            EditorUtility.SetDirty(controller);
        }

        private static void ConfigureSeatedActing(AnimatorController controller)
        {
            // The chair pose owns hips/legs; standing speech and crying supply only arms/head.
            var baseTalk = controller.layers[0].stateMachine.states.First(s => s.state.name == "SittingTalking").state;
            baseTalk.motion = Clip("Sitting Idle");
            const string layerName = "Seated acting";
            if (controller.layers.Any(l => l.name == layerName)) return;
            var mask = AssetDatabase.LoadAllAssetsAtPath(ControllerPath).OfType<AvatarMask>()
                .FirstOrDefault(m => m.name == "Seated dialogue upper body");
            if (mask == null)
            {
                mask = new AvatarMask { name = "Seated dialogue upper body" };
                AssetDatabase.AddObjectToAsset(mask, controller);
            }
            for (int i = 0; i < (int)AvatarMaskBodyPart.LastBodyPart; i++)
                mask.SetHumanoidBodyPartActive((AvatarMaskBodyPart)i, false);
            foreach (var part in new[] { AvatarMaskBodyPart.Head, AvatarMaskBodyPart.LeftArm,
                AvatarMaskBodyPart.RightArm, AvatarMaskBodyPart.LeftFingers, AvatarMaskBodyPart.RightFingers })
                mask.SetHumanoidBodyPartActive(part, true);
            EditorUtility.SetDirty(mask);
            var machine = new AnimatorStateMachine { name = layerName };
            AssetDatabase.AddObjectToAsset(machine, controller);
            controller.AddLayer(new AnimatorControllerLayer { name = layerName, stateMachine = machine,
                avatarMask = mask, defaultWeight = 1, blendingMode = AnimatorLayerBlendingMode.Override });
            var silent = machine.AddState("Silent"); machine.defaultState = silent;
            foreach (bool crying in new[] { false, true })
            {
                var state = machine.AddState(crying ? "Crying" : "Talking");
                state.motion = Clip(crying ? "Crying" : "Talking");
                var enter = machine.AddAnyStateTransition(state);
                enter.hasExitTime = false; enter.canTransitionToSelf = false;
                enter.hasFixedDuration = true; enter.duration = .2f;
                enter.AddCondition(AnimatorConditionMode.If, 0, "IsSitting");
                enter.AddCondition(AnimatorConditionMode.If, 0, "DialogueTalking");
                enter.AddCondition(crying ? AnimatorConditionMode.Equals : AnimatorConditionMode.NotEqual, 1, "DialogueEmotion");
                foreach (string parameter in new[] { "IsSitting", "DialogueTalking" })
                {
                    var exit = state.AddTransition(silent);
                    exit.hasExitTime = false; exit.hasFixedDuration = true; exit.duration = .2f;
                    exit.AddCondition(AnimatorConditionMode.IfNot, 0, parameter);
                }
            }
            EditorUtility.SetDirty(controller);
        }

        [MenuItem("ThuyKieu/Chapter 1/Apply shared animations")]
        public static void Apply()
        {
            if (Application.isPlaying) throw new InvalidOperationException("Stop Play Mode first.");
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (scene.path != "Assets/_Game/Scenes/Chapter01_GiaBien.unity") throw new InvalidOperationException("Open Chapter 1 first.");
            foreach (string name in Names) if (!Clip(name).isHumanMotion) throw new InvalidOperationException("Import Shared clips as Humanoid first: " + name);
            var controller = GetController();
            var playerController = ThuyKieuLocomotionSetup.GetController(controller);
            var director = UnityEngine.Object.FindAnyObjectByType<Chapter01Director>();
            var serialized = new SerializedObject(director);
            var player = (PlayerMovement)serialized.FindProperty("_player").objectReferenceValue;
            var actors = new[] { player.GetComponentInChildren<Animator>(), (Animator)serialized.FindProperty("_motherAnimator").objectReferenceValue,
                (Animator)serialized.FindProperty("_maAnimator").objectReferenceValue };
            foreach (var animator in actors)
            {
                if (animator == null || animator.avatar == null || !animator.avatar.isHuman || !animator.avatar.isValid)
                    throw new InvalidOperationException("Actor needs a valid Humanoid avatar.");
                animator.runtimeAnimatorController = animator == actors[0] ? playerController : controller; animator.applyRootMotion = false;
                animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            }
            const string playerPrefabPath = "Assets/_Game/Prefabs/Player/ThuyKieu.prefab";
            var prefab = PrefabUtility.LoadPrefabContents(playerPrefabPath);
            try
            {
                var prefabAnimator = prefab.GetComponentInChildren<Animator>();
                prefabAnimator.runtimeAnimatorController = playerController; prefabAnimator.applyRootMotion = false;
                ThuyKieuLocomotionSetup.ConfigureMovement(prefab.GetComponent<PlayerMovement>());
                PrefabUtility.SaveAsPrefabAsset(prefab, playerPrefabPath);
            }
            finally { PrefabUtility.UnloadPrefabContents(prefab); }
            // Saving the prefab can refresh instances and restore their earlier controller override.
            foreach (var animator in new[] { player.GetComponentInChildren<Animator>(), actors[1], actors[2] })
            {
                animator.runtimeAnimatorController = animator == player.GetComponentInChildren<Animator>() ? playerController : controller; animator.applyRootMotion = false;
                if (PrefabUtility.IsPartOfPrefabInstance(animator)) PrefabUtility.RecordPrefabInstancePropertyModifications(animator);
                EditorUtility.SetDirty(animator);
            }
            ThuyKieuLocomotionSetup.ConfigureMovement(player);
            EditorSceneManager.MarkSceneDirty(scene); EditorSceneManager.SaveScene(scene); AssetDatabase.SaveAssets();
        }
    }
}
