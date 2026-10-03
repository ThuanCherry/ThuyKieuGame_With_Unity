using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThuyKieu.Dialogue;
using ThuyKieu.Interaction;
using ThuyKieu.Player;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace ThuyKieu.Core.Editor
{
    public static class Chapter01PlayModeCheck
    {
        [Serializable] private class Report { public bool completed; public bool passed = true; public List<string> checks = new List<string>(); }
        private static Report _report;
        private static Chapter01Director _director;
        private static DialogueManager _manager;
        private static DialogueUI _view;
        private static PlayerMovement _player;
        private static ThirdPersonCameraController _camera;
        private static Keyboard _keyboard;
        private static Keyboard _previousKeyboard;
        private static Key[] _keys;
        private static bool _pulse;
        private static int _phase, _branch;
        private static float _start;
        private static Vector3 _origin;
        private static InputSettings.BackgroundBehavior _background;
        private static bool _runInBackground;
        private static Animator _maAnimator;
        private static bool _maTalkingChecked, _maIdleChecked;
        private static bool _cryingChecked, _arguingChecked, _agreeingChecked, _disagreeChecked, _lookingChecked, _pickupChecked;
        public static bool Running { get; private set; }

        public static void Start()
        {
            if (!Application.isPlaying || UnityEngine.SceneManagement.SceneManager.GetActiveScene().path != Chapter01Setup.ScenePath)
                throw new InvalidOperationException("Open Chapter01 and enter Play Mode first.");
            if (Running) throw new InvalidOperationException("Check already running.");
            _director = UnityEngine.Object.FindAnyObjectByType<Chapter01Director>();
            _manager = DialogueManager.Instance;
            _view = UnityEngine.Object.FindAnyObjectByType<DialogueUI>();
            _player = UnityEngine.Object.FindAnyObjectByType<PlayerMovement>();
            _camera = UnityEngine.Object.FindAnyObjectByType<ThirdPersonCameraController>();
            _previousKeyboard = Keyboard.current;
            _keyboard = InputSystem.AddDevice<Keyboard>();
            _background = InputSystem.settings.backgroundBehavior;
            _runInBackground = Application.runInBackground;
            InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            Application.runInBackground = true;
            _keys = Array.Empty<Key>(); _phase = 0; _branch = 0; _start = Time.time;
            _report = new Report(); Running = true;
            _maAnimator = (Animator)new SerializedObject(_director).FindProperty("_maAnimator").objectReferenceValue;
            _maTalkingChecked = _maIdleChecked = false;
            _cryingChecked = _arguingChecked = _agreeingChecked = _disagreeChecked = _lookingChecked = _pickupChecked = false;
            Check(_maAnimator != null && _maAnimator.avatar.isValid && _maAnimator.avatar.isHuman && !_maAnimator.applyRootMotion, "Ma Giam Sinh valid Humanoid without root motion");
            var playerController = _player.GetComponentInChildren<Animator>().runtimeAnimatorController;
            var sharedController = playerController is AnimatorOverrideController gait ? gait.runtimeAnimatorController : playerController;
            Check(sharedController.animationClips.Length == 14 && sharedController.animationClips.All(c => c.isHumanMotion && AssetDatabase.GetAssetPath(c).Contains("/Animations/Shared/")), "All 14 controller clips use Shared Humanoid assets");
            Check(playerController is AnimatorOverrideController && _maAnimator.runtimeAnimatorController == sharedController, "Kieu uses a locomotion override while NPCs retain the shared controller");
            if (_manager.IsDialogueActive) _manager.EndDialogue();
            typeof(Chapter01Director).GetField("<CurrentStage>k__BackingField", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(_director, Chapter01Director.Stage.Opening);
            _manager.SetInkStory(new Ink.Runtime.Story(AssetDatabase.LoadAssetAtPath<TextAsset>(InkStoryCompilerPath).text));
            _manager.ResumeInk();
            InputSystem.onAfterUpdate += SupplyInput;
            EditorApplication.update += Tick;
            Application.logMessageReceived += CaptureLog;
        }

        private static void SupplyInput()
        {
            if (InputState.currentUpdateType == InputUpdateType.Dynamic && _keyboard != null)
            {
                _keyboard.MakeCurrent();
                InputState.Change(_keyboard, new KeyboardState(_keys));
                if (_pulse) { _keys = Array.Empty<Key>(); _pulse = false; }
            }
        }
        private static void CaptureLog(string message, string trace, LogType type)
        { if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert) { _report.passed = false; _report.checks.Add("ERROR: " + message); } }
        private static void Check(bool condition, string name)
        { _report.checks.Add((condition ? "PASS: " : "FAIL: ") + name); _report.passed &= condition; }
        private static void Teleport(Vector3 position)
        { var cc = _player.GetComponent<CharacterController>(); cc.enabled = false; _player.transform.position = position; cc.enabled = true; Physics.SyncTransforms(); }
        private static void Next(int phase) { _phase = phase; _start = Time.time; _keys = Array.Empty<Key>(); }
        private static void ContinueDisplayedLine()
        {
            if (_view.IsRevealing) _view.OnContinueClicked();
            _view.OnContinueClicked();
        }
        private static void Tick()
        {
            if (!Application.isPlaying) { Finish(); return; }
            try
            {
                float elapsed = Time.time - _start;
                if (elapsed > 25) throw new InvalidOperationException("Phase timed out: " + _phase + " " + _director.CurrentStage);
                switch (_phase)
                {
                    case 0:
                        if (elapsed < 2 || !_manager.IsDialogueActive) return;
                        Check(_view.CurrentText.StartsWith("Mưa quất"), "Opening text and UI: " + _view.CurrentText);
                        Check(_player.ControlLocked, "Opening locks movement");
                        _origin = _player.transform.position; _keys = new[] { Key.W }; Next(1); _keys = new[] { Key.W }; break;
                    case 1:
                        if (elapsed < 0.4f) return;
                        Check(Vector3.Distance(_origin, _player.transform.position) < 0.1f, "W cannot move during dialogue");
                        if (_view.IsRevealing) _view.OnContinueClicked();
                        Next(2); _keys = new[] { Key.Space }; _pulse = true; break;
                    case 2:
                        if (elapsed < 0.15f) return;
                        Check(_view.CurrentText.StartsWith("Vương Ông"), "Space advances dialogue: " + _view.CurrentText);
                        _keys = Array.Empty<Key>(); ContinueDisplayedLine(); Next(3); break;
                    case 3:
                        Check(_director.CurrentStage == Chapter01Director.Stage.Mother && !_player.ControlLocked, "Opening returns movement at Mother stage");
                        _camera.enabled = false; Camera.main.transform.rotation = Quaternion.identity;
                        Teleport(new Vector3(0, 0.05f, -0.5f)); _origin = _player.transform.position;
                        Next(4); _keys = new[] { Key.W }; break;
                    case 4:
                        if (elapsed < 0.55f) return;
                        Check(_player.CurrentSpeed > 1.5f && _player.transform.position.z > _origin.z + 0.7f, "WASD walks with CharacterController");
                        Check(_player.GetComponentInChildren<Animator>().GetFloat("Speed") > 0.2f, "Walk Animator responds");
                        Check(_player.GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0).Any(c => c.clip.name == "TK-Walking" && c.weight > .8f), "Kieu plays her own walking clip");
                        Teleport(new Vector3(3.8f, 0.05f, -0.5f));
                        Next(5); _keys = new[] { Key.W, Key.LeftShift }; break;
                    case 5:
                        if (elapsed < 0.5f) return;
                        Check(_player.IsRunning && _player.CurrentSpeed > 4, "Shift runs");
                        Check(_player.GetComponentInChildren<Animator>().GetFloat("Speed") > 0.7f, "Run Animator responds");
                        Check(_player.GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0).Any(c => c.clip.name == "TK-Running" && c.weight > .8f), "Kieu plays her own running clip");
                        Teleport(new Vector3(0, 0.05f, 5.5f)); Next(6); _keys = new[] { Key.W }; break;
                    case 6:
                        if (elapsed < 0.8f) return;
                        Check(_player.transform.position.z < 6.3f && _player.transform.position.y > -0.15f, "Wall and floor collision");
                        Teleport(new Vector3(-2.5f, 0.05f, 0.2f)); Next(7); break;
                    case 7:
                        if (elapsed < 0.15f) return;
                        var idleAnimator = _player.GetComponentInChildren<Animator>();
                        idleAnimator.Update(0.3f); idleAnimator.Update(0.3f);
                        Check(idleAnimator.GetFloat("Speed") < 0.05f && idleAnimator.GetCurrentAnimatorClipInfo(0).Any(c => c.clip.name == "Idle" && c.weight > 0.9f), "Thuy Kieu uses new Shared Idle while standing");
                        Check(_player.GetComponent<PlayerInteraction>().CurrentTarget != null, "Interaction range finds Mother");
                        Next(8); _keys = new[] { Key.E }; _pulse = true; break;
                    case 8:
                        if (elapsed < 0.15f) return;
                        Check(_manager.IsDialogueActive && _view.CurrentName == "Mẹ Kiều", "E opens Mother dialogue");
                        Next(9); break;
                    case 9:
                        if (elapsed < 0.08f) return;
                        _start = Time.time;
                        VerifyGesture("Crying", 1, ref _cryingChecked);
                        VerifyGesture("Arguing", 2, ref _arguingChecked);
                        VerifyGesture("Agreeing", 3, ref _agreeingChecked);
                        VerifyGesture("Disagree", 4, ref _disagreeChecked);
                        VerifyGesture("LookingAround", 5, ref _lookingChecked);
                        VerifyGesture("Pickup", 6, ref _pickupChecked);
                        if (_maAnimator != null && _maAnimator.gameObject.activeInHierarchy)
                        {
                            bool speaking = _view.CurrentName == "Mã Giám Sinh" && _manager.IsDialogueActive;
                            if (speaking && !_maTalkingChecked)
                            {
                                _maAnimator.Update(0.3f); _maAnimator.Update(0.3f);
                                Check(_maAnimator.GetBool("DialogueTalking") && _maAnimator.GetCurrentAnimatorStateInfo(0).IsName("Talking"), "Ma Giam Sinh Talking state follows Ink speaker");
                                _maTalkingChecked = true;
                            }
                            if (!speaking && _maTalkingChecked && !_maIdleChecked)
                            {
                                _maAnimator.Update(0.3f); _maAnimator.Update(0.3f);
                                Check(!_maAnimator.GetBool("DialogueTalking") && _maAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"), "Ma Giam Sinh returns to Idle for other speakers");
                                _maIdleChecked = true;
                            }
                        }
                        if (_manager.IsDialogueActive)
                        {
                            if (_manager.IsAtChoicePoint)
                            {
                                int index = _director.CurrentStage == Chapter01Director.Stage.Negotiation ? _branch / 2 : _branch % 2;
                                int count = _director.CurrentStage == Chapter01Director.Stage.Negotiation ? 3 : 2;
                                Check(_view.VisibleChoiceCount == count, "Branch " + _branch + " has " + count + " UI choice buttons");
                                string before = _view.CurrentText; _view.OnContinueClicked();
                                Check(_view.CurrentText == before && _manager.IsAtChoicePoint, "Continue cannot skip choices");
                                _view.OnChoiceClicked(index);
                            }
                            else _view.OnContinueClicked();
                        }
                        else if (_director.CurrentStage == Chapter01Director.Stage.Contract || _director.CurrentStage == Chapter01Director.Stage.Exit)
                        {
                            Check(!_player.ControlLocked, "Movement unlocked for " + _director.CurrentStage);
                            var target = UnityEngine.Object.FindObjectsByType<ChapterInteractable>().First(i => i.IsAvailable);
                            target.Interact();
                        }
                        else if (_director.CurrentStage == Chapter01Director.Stage.Complete) Next(10);
                        else throw new InvalidOperationException("Unexpected idle stage: " + _director.CurrentStage);
                        break;
                    case 10:
                        int expected = (_branch / 2 == 2 ? 2 : 1) + (_branch % 2 == 0 ? 1 : 0);
                        Check((int)_manager.InkStory.variablesState["tinh_tao"] == expected, "Branch " + _branch + " tinh_tao=" + expected);
                        Check((int)_manager.InkStory.variablesState["tu_trong"] == (_branch % 2 == 0 ? 2 : 1), "Branch " + _branch + " tu_trong correct");
                        Check(!string.IsNullOrEmpty(InkStatePersistence.Instance.StateJson), "State persisted for Chapter 2");
                        Check(UnityEngine.Object.FindObjectsByType<PlayerMovement>().Length == 1, "Single player after dialogue");
                        Check(_view.CurrentText.Contains("chìa khóa"), "Final Chapter 1 line shown");
                        if (++_branch == 6)
                        {
                            Check(_maTalkingChecked && _maIdleChecked, "Ma Giam Sinh Idle and Talking both exercised");
                            Check(_cryingChecked && _arguingChecked && _agreeingChecked && _disagreeChecked && _lookingChecked && _pickupChecked, "All Chapter 1 emotion and interaction animations exercised");
                            VerifySeatedAnimations();
                            var environmentAudio = UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Environment.Chapter01Audio>();
                            Check(UnityEngine.Object.FindObjectsByType<AudioSource>().All(s => environmentAudio != null && s.transform.IsChildOf(environmentAudio.transform)), "Audio sources are environment/UI only; no dialogue or character voice sources");
                            var font = UnityEngine.Object.FindObjectsByType<TMP_Text>(FindObjectsInactive.Include).First().font;
                            Check("Thúy Kiều Mẹ Mã Giám Sinh CHƯƠNG HOÀN THÀNH".All(c => font.HasCharacter(c)), "Vietnamese glyph coverage");
                            _director.ContinueAfterChapter(); Next(11);
                        }
                        else
                        {
                            typeof(Chapter01Director).GetField("<CurrentStage>k__BackingField", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(_director, Chapter01Director.Stage.Opening);
                            GameObject.Find("DialogueCanvas/ChapterComplete").SetActive(false);
                            _manager.SetInkStory(new Ink.Runtime.Story(AssetDatabase.LoadAssetAtPath<TextAsset>(InkStoryCompilerPath).text));
                            _manager.ResumeInk(); _manager.Advance(); _manager.Advance();
                            _director.Interact(Chapter01Director.Stage.Mother, _player.transform); Next(9);
                        }
                        break;
                    case 11:
                        if (elapsed < 0.5f) return;
                        Check(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Chapter02_Placeholder", "Continue loads Chapter 2 placeholder");
                        Check(InkStatePersistence.Instance != null && !string.IsNullOrEmpty(InkStatePersistence.Instance.StateJson), "Session survives scene transition");
                        Finish(); break;
                }
            }
            catch (Exception exception) { Check(false, exception.ToString()); Finish(); }
        }
        private const string InkStoryCompilerPath = "Assets/_Game/Data/Dialogue/Ink/Main.json";
        private static void VerifyGesture(string state, int emotion, ref bool verified)
        {
            if (verified) return;
            foreach (var animator in UnityEngine.Object.FindObjectsByType<Animator>())
            {
                if (!animator.GetBool("DialogueTalking") || animator.GetInteger("DialogueEmotion") != emotion) continue;
                animator.Update(0.22f); animator.Update(0.22f);
                Check(animator.GetCurrentAnimatorStateInfo(0).IsName(state), "Shared " + state + " follows Chapter 1 cues");
                verified = true;
                break;
            }
        }
        private static void VerifySeatedAnimations()
        {
            Vector3 position = _maAnimator.transform.position;
            _maAnimator.SetBool("DialogueTalking", false);
            _maAnimator.SetBool("IsSitting", true); _maAnimator.SetTrigger("Sit");
            _maAnimator.Update(0.1f); _maAnimator.Update(0.2f);
            Check(_maAnimator.GetCurrentAnimatorStateInfo(0).IsName("StandToSit"), "Shared Stand To Sit transition");
            for (int i = 0; i < 14; i++) _maAnimator.Update(0.25f);
            Check(_maAnimator.GetCurrentAnimatorStateInfo(0).IsName("SittingIdle"), "Shared Sitting Idle");
            _maAnimator.SetBool("DialogueTalking", true);
            _maAnimator.Update(0.3f); _maAnimator.Update(0.3f);
            Check(_maAnimator.GetCurrentAnimatorStateInfo(0).IsName("SittingTalking"), "Shared Sitting Talking");
            _maAnimator.SetBool("DialogueTalking", false); _maAnimator.SetTrigger("Stand");
            _maAnimator.Update(0.1f); _maAnimator.Update(0.2f);
            Check(_maAnimator.GetCurrentAnimatorStateInfo(0).IsName("SitToStand"), "Shared Sit To Stand transition");
            for (int i = 0; i < 14; i++) _maAnimator.Update(0.25f);
            Check(_maAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !_maAnimator.GetBool("IsSitting") && Vector3.Distance(position, _maAnimator.transform.position) < 0.01f, "Seated sequence returns to Idle without moving the actor root");
        }
        private static void Finish()
        {
            if (!Running) return;
            Running = false; EditorApplication.update -= Tick; InputSystem.onAfterUpdate -= SupplyInput; Application.logMessageReceived -= CaptureLog;
            InputSystem.RemoveDevice(_keyboard); _keyboard = null;
            if (_previousKeyboard != null) _previousKeyboard.MakeCurrent();
            InputSystem.settings.backgroundBehavior = _background; Application.runInBackground = _runInBackground;
            if (_camera != null) _camera.enabled = true;
            _report.completed = true;
            Directory.CreateDirectory("Tools/Chapter01"); File.WriteAllText("Tools/Chapter01/PlayModeReport.json", JsonUtility.ToJson(_report, true));
        }
    }
}
