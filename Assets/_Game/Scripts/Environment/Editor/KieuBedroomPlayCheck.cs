using System;
using System.Collections.Generic;
using System.IO;
using ThuyKieu.Core;
using ThuyKieu.Dialogue;
using ThuyKieu.Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace ThuyKieu.Environment.Editor
{
    public static class KieuBedroomPlayCheck
    {
        private static Keyboard _keyboard, _previous;
        private static PlayerMovement _player;
        private static DialogueManager _dialogue;
        private static readonly List<string> Checks = new List<string>();
        private static readonly Vector3[] Waypoints = {new Vector3(-3.7f, 0, 1.9f), new Vector3(-2.5f, 0, .65f)};
        private static int _waypoint;
        private static float _start;
        private static bool _passed, _background;
        private static InputSettings.BackgroundBehavior _focus;
        private static Key[] _keys;

        public static void Start()
        {
            if (!Application.isPlaying) throw new InvalidOperationException("Enter Play Mode first.");
            _player = UnityEngine.Object.FindAnyObjectByType<PlayerMovement>();
            _dialogue = DialogueManager.Instance;
            Checks.Clear(); _passed = true; _waypoint = 0; _start = Time.realtimeSinceStartup;
            Check(Vector3.Distance(_player.transform.position, new Vector3(-3.7f, .05f, 4.5f)) < .3f, "Player starts beside bed");
            Check(_player.ControlLocked, "Opening locks movement");
            _previous = Keyboard.current; _keyboard = InputSystem.AddDevice<Keyboard>();
            _focus = InputSystem.settings.backgroundBehavior; _background = Application.runInBackground;
            InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            Application.runInBackground = true; _keys = Array.Empty<Key>();
            InputSystem.onAfterUpdate += Input; EditorApplication.update += Tick;
            Application.logMessageReceived += Log;
        }
        private static void Input()
        {
            if (InputState.currentUpdateType != InputUpdateType.Dynamic) return;
            _keyboard.MakeCurrent(); InputState.Change(_keyboard, new KeyboardState(_keys));
        }
        private static void Log(string message, string stack, LogType type)
        { if (type == LogType.Error || type == LogType.Exception) Check(false, message); }
        private static void Check(bool ok, string label) { _passed &= ok; Checks.Add((ok ? "PASS: " : "FAIL: ") + label); }
        private static void Tick()
        {
            if (!Application.isPlaying) { Check(false, "Play interrupted"); Finish(); return; }
            if (Time.realtimeSinceStartup - _start > 35) { Check(false, "Traversal timed out at " + _player.transform.position); Finish(); return; }
            if (_dialogue.IsDialogueActive) { _dialogue.Advance(); return; }
            if (_player.ControlLocked) return;
            if (_waypoint >= Waypoints.Length)
            {
                Check(_player.GetComponent<CharacterController>().isGrounded, "Grounded throughout bedroom to hall route");
                Check(_player.transform.position.y > -.15f, "No floor fallthrough");
                Check(UnityEngine.Object.FindAnyObjectByType<Chapter01Director>().CurrentStage == Chapter01Director.Stage.Mother, "Story waits for Mother interaction");
                Finish(); return;
            }
            Vector3 delta = Waypoints[_waypoint] - _player.transform.position; delta.y = 0;
            if (delta.magnitude < .23f) { Check(true, "Walked to waypoint " + _waypoint + " without teleport"); _waypoint++; _keys = Array.Empty<Key>(); return; }
            var camera = Camera.main.transform;
            var forward = camera.forward; forward.y = 0; forward.Normalize();
            var right = camera.right; right.y = 0; right.Normalize();
            float x = Vector3.Dot(delta.normalized, right), y = Vector3.Dot(delta.normalized, forward);
            var keys = new List<Key>();
            if (Mathf.Abs(x) > .35f) keys.Add(x > 0 ? Key.D : Key.A);
            if (Mathf.Abs(y) > .35f) keys.Add(y > 0 ? Key.W : Key.S);
            _keys = keys.ToArray();
        }
        private static void Finish()
        {
            EditorApplication.update -= Tick; InputSystem.onAfterUpdate -= Input; Application.logMessageReceived -= Log;
            InputSystem.RemoveDevice(_keyboard); if (_previous != null && _previous.added) _previous.MakeCurrent();
            InputSystem.settings.backgroundBehavior = _focus; Application.runInBackground = _background;
            File.WriteAllText("Tools/Chapter01/BedroomPlayReport.json", Newtonsoft.Json.JsonConvert.SerializeObject(new {passed = _passed, checks = Checks}, Newtonsoft.Json.Formatting.Indented));
        }
    }
}
