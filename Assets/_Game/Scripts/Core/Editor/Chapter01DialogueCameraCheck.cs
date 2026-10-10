using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThuyKieu.Dialogue;
using ThuyKieu.Interaction;
using ThuyKieu.Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace ThuyKieu.Core.Editor
{
    public static class Chapter01DialogueCameraCheck
    {
        private static readonly List<string> Checks = new List<string>();
        private static readonly HashSet<string> Seen = new HashSet<string>();
        private static Chapter01Director _director;
        private static DialogueManager _dialogue;
        private static ThirdPersonCameraController _camera;
        private static PlayerMovement _player;
        private static PlayerInteraction _interaction;
        private static Keyboard _keyboard, _previous;
        private static Mouse _mouse, _previousMouse;
        private static Key[] _keys;
        private static float _start, _next, _lastShotTime;
        private static float _gameplayFov;
        private static Vector3 _resumePosition;
        private static Quaternion _resumeRotation;
        private static bool _resumeProbe;
        private static bool _passed, _stand, _entered, _ended, _background;
        private static int _waypoint;
        private static string _shot;
        private static Vector3 _lockedPosition;
        private static InputSettings.BackgroundBehavior _focus;
        public static bool Running { get; private set; }

        public static void Start()
        {
            if (!Application.isPlaying || Running) throw new InvalidOperationException("Run once in fresh Play Mode.");
            Checks.Clear(); Seen.Clear(); _passed = true; _stand = _entered = _ended = false; _waypoint = 0; _shot = null;
            _resumeProbe = false;
            _director = UnityEngine.Object.FindAnyObjectByType<Chapter01Director>();
            _dialogue = DialogueManager.Instance;
            _camera = UnityEngine.Object.FindAnyObjectByType<ThirdPersonCameraController>();
            _gameplayFov = Camera.main.fieldOfView;
            _player = UnityEngine.Object.FindAnyObjectByType<PlayerMovement>();
            _interaction = _player.GetComponent<PlayerInteraction>();
            _previous = Keyboard.current; _keyboard = InputSystem.AddDevice<Keyboard>(); _keys = Array.Empty<Key>();
            _previousMouse = Mouse.current; _mouse = InputSystem.AddDevice<Mouse>();
            _background = Application.runInBackground; _focus = InputSystem.settings.backgroundBehavior;
            Application.runInBackground = true; InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            _start = Time.realtimeSinceStartup; _next = 0; Running = true;
            InputSystem.onAfterUpdate += Supply; EditorApplication.update += Tick; Application.logMessageReceived += Log;
        }
        private static void Supply()
        {
            if (InputState.currentUpdateType != InputUpdateType.Dynamic) return;
            _keyboard.MakeCurrent(); InputState.Change(_keyboard, new KeyboardState(_keys));
            _mouse.MakeCurrent();
            InputState.Change(_mouse, new MouseState { delta = _entered && (!_ended || _resumeProbe) ? new Vector2(12, 0) : Vector2.zero });
        }
        private static void Check(bool ok, string label) { _passed &= ok; Checks.Add((ok ? "PASS: " : "FAIL: ") + label); }
        private static void Once(bool ok, string label) { if (Seen.Add(label)) Check(ok, label); }
        private static void Log(string message, string stack, LogType type)
        { if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert) Check(false, "Runtime: " + message); }
        private static void Tick()
        {
            try
            {
                if (!Application.isPlaying) throw new InvalidOperationException("Play interrupted");
                if (Time.realtimeSinceStartup - _start > 100) throw new InvalidOperationException("Camera path timeout at " + _director.CurrentStage + " " + _player.transform.position);
                if (_director.MotherConversation && _dialogue.IsDialogueActive)
                {
                    if (!_entered)
                    {
                        _entered = true; _lockedPosition = _player.transform.position;
                        Check(_camera.CurrentDialogueCue != null && _camera.CurrentDialogueCue.name == "Dialogue_TwoShot", "Conversation starts in TwoShot");
                        Check(_player.ControlLocked && _interaction.ControlLocked && _camera.ControlLocked, "Movement, interaction and look locked");
                    }
                    _keys = new[] { Key.W, Key.D };
                    Once(Vector3.Distance(_lockedPosition, _player.transform.position) < .08f, "Movement input cannot move Kiều during dialogue");
                    Once(UnityEngine.Object.FindObjectsByType<AudioListener>(FindObjectsSortMode.None).Count(a => a.isActiveAndEnabled) == 1, "Exactly one active AudioListener");
                    Once(GameObject.Find("MeKieu").GetComponentInChildren<Animator>().GetBool("IsSitting"), "Mother stays seated");
                    string shot = _camera.CurrentDialogueCue != null ? _camera.CurrentDialogueCue.name : "none";
                    if (shot != _shot) { _shot = shot; _lastShotTime = Time.time; _next = Time.realtimeSinceStartup + 1.1f; }
                    if (!_camera.IsBlending && Time.time - _lastShotTime > .15f && Seen.Add("image:" + shot))
                    {
                        Check(Vector3.Distance(_camera.transform.position, _camera.CurrentDialogueCue.position) < .08f, shot + " blend reaches preset");
                        Check(!Physics.CheckSphere(_camera.transform.position, .12f, 1 << 8, QueryTriggerInteraction.Ignore), shot + " lens clear of walls/furniture");
                        Check(Camera.main.fieldOfView >= 35 && Camera.main.fieldOfView <= 50, shot + " cinematic FOV");
                        Check(Quaternion.Angle(_camera.transform.rotation, _camera.CurrentDialogueCue.rotation) < .1f, shot + " ignores gameplay mouse delta");
                        if (shot == "Dialogue_MeKieu")
                        {
                            var router = _director.GetComponent<DialogueTagRouter>();
                            var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
                            var route = typeof(DialogueTagRouter).GetMethod("RouteMotherLine", flags);
                            var tick = typeof(DialogueTagRouter).GetMethod("LateUpdate", flags);
                            route.Invoke(router, new object[] { new[] { "speaker:Kieu", "camera:MCU_MeKieu" } }); tick.Invoke(router, null);
                            Check(_camera.CurrentDialogueCue.name == shot && !_camera.IsBlending, "Explicit camera tag overrides speaker; repeated shot does not restart blend");
                            route.Invoke(router, new object[] { new[] { "speaker:MeKieu", "camera:Unknown_Camera" } }); tick.Invoke(router, null);
                            Check(_camera.CurrentDialogueCue.name == shot && !_camera.IsBlending, "Unknown camera tag safely falls back to speaker");
                        }
                        foreach (var actor in new[] { GameObject.Find("MeKieu"), _player.gameObject })
                        {
                            if (shot == "Dialogue_MeKieu" && actor == _player.gameObject || shot == "Dialogue_Kieu" && actor != _player.gameObject) continue;
                            var head = actor.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.Head).position;
                            var viewport = Camera.main.WorldToViewportPoint(head);
                            Check(viewport.z > 0 && viewport.x > .04f && viewport.x < .96f && viewport.y > .45f && viewport.y < .94f, shot + " " + actor.name + " face above dialogue box " + viewport);
                            Check(!Physics.Linecast(_camera.transform.position, head, 1 << 8, QueryTriggerInteraction.Ignore), shot + " " + actor.name + " unobstructed face");
                        }
                        ScreenCapture.CaptureScreenshot("Tools/Chapter01/" + shot + "_" + Screen.width + "x" + Screen.height + ".png");
                    }
                    if (Time.realtimeSinceStartup >= _next)
                    {
                        _next = Time.realtimeSinceStartup + .9f;
                        _dialogue.Advance();
                    }
                    return;
                }
                if (_entered)
                {
                    _keys = _resumeProbe ? new[] { Key.S } : Array.Empty<Key>();
                    if (!_ended)
                    {
                        _ended = true; _next = Time.realtimeSinceStartup + 1.1f;
                        Check(!_player.ControlLocked && !_camera.ControlLocked && !_interaction.ControlLocked, "Gameplay input restored at end");
                        Check(_camera.CurrentDialogueCue == null, "Dialogue cue released");
                        Check(Seen.Contains("image:Dialogue_MeKieu") && Seen.Contains("image:Dialogue_Kieu") && Seen.Contains("image:Dialogue_TwoShot"), "All three authored conversation shots exercised");
                    }
                    if (Time.realtimeSinceStartup < _next) return;
                    if (!_resumeProbe)
                    {
                        _resumeProbe = true; _resumePosition = _player.transform.position; _resumeRotation = _camera.transform.rotation;
                        _keys = new[] { Key.S }; _next = Time.realtimeSinceStartup + .3f; return;
                    }
                    Check(Vector3.Distance(_resumePosition, _player.transform.position) > .05f, "Movement responds after dialogue");
                    Check(Quaternion.Angle(_resumeRotation, _camera.transform.rotation) > 1, "Mouse look responds after dialogue");
                    Check(!_camera.IsBlending && Mathf.Abs(Camera.main.fieldOfView - _gameplayFov) < .1f, "Return blend completes and gameplay FOV restored");
                    Finish(); return;
                }
                if (_dialogue.IsDialogueActive)
                {
                    _keys = Array.Empty<Key>();
                    if (Time.realtimeSinceStartup >= _next) { _next = Time.realtimeSinceStartup + .15f; _dialogue.Advance(); }
                    return;
                }
                if (_director.CurrentStage == Chapter01Director.Stage.Hallway)
                {
                    if (!_stand) { GameObject.Find("OpeningStandButton").GetComponent<UnityEngine.UI.Button>().onClick.Invoke(); _stand = true; }
                    if (_player.ControlLocked) return;
                    if (Walk(new Vector3(-3.7f, 0, 2.75f))) _director.Interact(Chapter01Director.Stage.Hallway, GameObject.Find("KieuRoomDoor").transform);
                }
                else if (_director.CurrentStage == Chapter01Director.Stage.Mother)
                {
                    var route = new[] { new Vector3(-3.7f, 0, 1.55f), new Vector3(-.55f, 0, 1.45f) };
                    if (_waypoint < route.Length) { if (Walk(route[_waypoint])) _waypoint++; }
                    else
                    {
                        _keys = Array.Empty<Key>();
                        Check(Vector3.Distance(_player.transform.position, GameObject.Find("MeKieu").transform.position) < 2.5f, "Player physically walks into mother's interaction range");
                        _director.Interact(Chapter01Director.Stage.Mother, GameObject.Find("MeKieu").transform);
                    }
                }
            }
            catch (Exception error) { Check(false, error.ToString()); Finish(); }
        }
        private static bool Walk(Vector3 target)
        {
            Vector3 delta = target - _player.transform.position; delta.y = 0;
            if (delta.magnitude < .16f) { _keys = Array.Empty<Key>(); return true; }
            Vector3 forward = Camera.main.transform.forward, right = Camera.main.transform.right;
            forward.y = right.y = 0; forward.Normalize(); right.Normalize();
            float x = Vector3.Dot(delta.normalized, right), y = Vector3.Dot(delta.normalized, forward);
            var keys = new List<Key>();
            if (Mathf.Abs(x) > .35f) keys.Add(x > 0 ? Key.D : Key.A);
            if (Mathf.Abs(y) > .35f) keys.Add(y > 0 ? Key.W : Key.S);
            _keys = keys.ToArray(); return false;
        }
        private static void Finish()
        {
            Running = false; InputSystem.onAfterUpdate -= Supply; EditorApplication.update -= Tick; Application.logMessageReceived -= Log;
            InputSystem.RemoveDevice(_keyboard); if (_previous != null && _previous.added) _previous.MakeCurrent();
            InputSystem.RemoveDevice(_mouse); if (_previousMouse != null && _previousMouse.added) _previousMouse.MakeCurrent();
            Application.runInBackground = _background; InputSystem.settings.backgroundBehavior = _focus;
            File.WriteAllText("Tools/Chapter01/DialogueCameraReport_" + Screen.width + "x" + Screen.height + ".json", Newtonsoft.Json.JsonConvert.SerializeObject(new { passed = _passed, checks = Checks }, Newtonsoft.Json.Formatting.Indented));
        }
    }
}
