using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace ThuyKieu.Player.Editor
{
    /// <summary>Opt-in integration check using real Input System keyboard state and gameplay updates.</summary>
    public static class ThuyKieuPlayModeCheck
    {
        [Serializable]
        private class Result
        {
            public string name;
            public bool passed;
            public float speed;
            public float animatorSpeed;
            public string position;
            public string detail;
        }

        [Serializable]
        private class Report
        {
            public bool completed;
            public bool passed;
            public List<Result> checks = new List<Result>();
        }

        private static PlayerMovement _player;
        private static Animator _animator;
        private static CharacterController _controller;
        private static ThirdPersonCameraController _camera;
        private static Quaternion _cameraRotation;
        private static Vector3 _startPosition;
        private static Quaternion _startRotation;
        private static InputSettings.BackgroundBehavior _backgroundBehavior;
        private static bool _runInBackground;
        private static int _phase;
        private static float _phaseStart;
        private static float _normalizedTime;
        private static Report _report;
        public static bool IsRunning { get; private set; }

        [MenuItem("ThuyKieu/Player/Validate playground in Play Mode")]
        public static void Start()
        {
            if (!Application.isPlaying || UnityEngine.SceneManagement.SceneManager.GetActiveScene().path != ThuyKieuPlayableSetup.ScenePath)
                throw new InvalidOperationException("Open ThuyKieu_Playground and enter Play Mode first.");
            if (IsRunning) throw new InvalidOperationException("Validation is already running.");
            _player = UnityEngine.Object.FindAnyObjectByType<PlayerMovement>();
            _animator = _player.GetComponentInChildren<Animator>();
            _controller = _player.GetComponent<CharacterController>();
            _camera = Camera.main.GetComponent<ThirdPersonCameraController>();
            _cameraRotation = Camera.main.transform.rotation;
            _startPosition = _player.transform.position;
            _startRotation = _player.transform.rotation;
            _backgroundBehavior = InputSystem.settings.backgroundBehavior;
            _runInBackground = Application.runInBackground;
            InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            Application.runInBackground = true;
            _camera.enabled = false;
            Camera.main.transform.rotation = Quaternion.identity;
            _report = new Report();
            _phase = 0;
            _phaseStart = Time.time;
            IsRunning = true;
            InputSystem.onAfterUpdate += SupplyKeyboard;
            EditorApplication.update += Tick;
            Application.logMessageReceived += CaptureErrors;
        }

        private static void SupplyKeyboard()
        {
            if (InputState.currentUpdateType != InputUpdateType.Dynamic || Keyboard.current == null) return;
            Key[] keys;
            switch (_phase)
            {
                case 1: case 8: keys = new[] { Key.W }; break;
                case 2: keys = new[] { Key.A }; break;
                case 3: keys = new[] { Key.S }; break;
                case 4: keys = new[] { Key.D }; break;
                case 5: keys = new[] { Key.W, Key.LeftShift }; break;
                case 6: keys = new[] { Key.W, Key.D }; break;
                case 10: keys = new[] { Key.C }; break;
                default: keys = Array.Empty<Key>(); break;
            }
            InputState.Change(Keyboard.current, new KeyboardState(keys));
        }

        private static void Tick()
        {
            if (!Application.isPlaying || _player == null)
            {
                Finish(false);
                return;
            }
            if (Time.time - _phaseStart < 0.9f) return;
            try
            {
                float speed = _player.CurrentSpeed;
                float animationSpeed = _animator.GetFloat("Speed");
                Vector3 position = _player.transform.position;
                switch (_phase)
                {
                    case 0:
                        Check("Idle and ground contact", speed < 0.02f && animationSpeed < 0.02f && _controller.isGrounded && position.y > -0.06f);
                        Check("One Humanoid model; root motion disabled", _animator.isHuman && _animator.avatar.isValid && !_animator.applyRootMotion
                            && UnityEngine.Object.FindObjectsByType<Animator>().Length == 1);
                        Check("Required mapped Humanoid bones", new[] { HumanBodyBones.Hips, HumanBodyBones.Spine, HumanBodyBones.Head,
                            HumanBodyBones.LeftUpperArm, HumanBodyBones.RightUpperArm, HumanBodyBones.LeftLowerArm, HumanBodyBones.RightLowerArm,
                            HumanBodyBones.LeftUpperLeg, HumanBodyBones.RightUpperLeg, HumanBodyBones.LeftLowerLeg, HumanBodyBones.RightLowerLeg }
                            .All(b => _animator.GetBoneTransform(b) != null));
                        _normalizedTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                        break;
                    case 1:
                        Check("W walks forward", position.z > 1f && InRange(speed, 2.5f) && InRange(animationSpeed, 0.5f, 0.03f));
                        Check("Locomotion time advances without per-frame restart", _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > _normalizedTime + 0.02f);
                        break;
                    case 2: Check("A walks left and turns", position.x < -1f && InRange(speed, 2.5f) && Vector3.Dot(_player.transform.forward, Vector3.left) > 0.95f); break;
                    case 3: Check("S walks backward and turns", position.z < -1f && InRange(speed, 2.5f) && Vector3.Dot(_player.transform.forward, Vector3.back) > 0.95f); break;
                    case 4: Check("D walks right and turns", position.x > 1f && InRange(speed, 2.5f) && Vector3.Dot(_player.transform.forward, Vector3.right) > 0.95f); break;
                    case 5: Check("Shift runs at code-controlled speed", position.z > 2.5f && InRange(speed, 5.5f) && InRange(animationSpeed, 1f, 0.03f) && _player.IsRunning); break;
                    case 6: Check("Diagonal does not gain speed", InRange(speed, 2.5f) && position.x > 1f && position.z > 1f); break;
                    case 7: Check("Released input returns to Idle", speed < 0.02f && animationSpeed < 0.02f && !_player.IsRunning); break;
                    case 8: Check("Wall blocks movement and actual velocity returns Idle", position.z < 7.55f && speed < 0.03f && animationSpeed < 0.02f); break;
                    case 9: Check("Gravity lands on floor", _controller.isGrounded && position.y > -0.06f && position.y < 0.08f); break;
                    case 10: Check("C safely ignored without crouch clips", speed < 0.02f && !_animator.GetBool("IsCrouching")); break;
                    case 11: Check("Talk trigger uses existing clip", _animator.GetCurrentAnimatorStateInfo(0).IsName("Talking") && _animator.GetCurrentAnimatorClipInfo(0).Any(c => c.clip.name == "TK-Talking")); break;
                    case 12: Check("Pickup trigger uses non-looping clip", _animator.GetCurrentAnimatorStateInfo(0).IsName("Pickup") && _animator.GetCurrentAnimatorClipInfo(0).Any(c => c.clip.name == "TK-Picking Up Object" && !c.clip.isLooping)); break;
                    case 13: Check("Pickup returns to locomotion", _animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")); Finish(true); return;
                }
                _phase++;
                if (_phase <= 6) Teleport(Vector3.up * 0.02f);
                if (_phase == 8) Teleport(new Vector3(0f, 0.02f, 7.45f));
                if (_phase == 9) Teleport(Vector3.up * 2f);
                if (_phase == 11) _animator.SetTrigger("Talk");
                if (_phase == 12)
                {
                    _animator.Play("Locomotion", 0, 0f);
                    _animator.Update(0f);
                    _animator.SetTrigger("Pickup");
                }
                _phaseStart = Time.time;
                if (_phase == 13) _phaseStart += 3f;
            }
            catch (Exception exception)
            {
                Check("Validation exception", false, exception.ToString());
                Finish(false);
            }
        }

        private static bool InRange(float actual, float expected, float tolerance = 0.12f) => Mathf.Abs(actual - expected) < tolerance;

        private static void Teleport(Vector3 position)
        {
            _controller.enabled = false;
            _player.transform.SetPositionAndRotation(position, Quaternion.identity);
            _controller.enabled = true;
            Physics.SyncTransforms();
        }

        private static void Check(string name, bool passed, string detail = "")
        {
            _report.checks.Add(new Result { name = name, passed = passed, detail = detail, speed = _player.CurrentSpeed,
                animatorSpeed = _animator.GetFloat("Speed"), position = _player.transform.position.ToString("F3") });
        }

        private static void CaptureErrors(string message, string stack, LogType type)
        {
            if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
                _report.checks.Add(new Result { name = "Runtime error", passed = false, detail = message + "\n" + stack });
        }

        private static void Finish(bool completed)
        {
            InputSystem.onAfterUpdate -= SupplyKeyboard;
            EditorApplication.update -= Tick;
            Application.logMessageReceived -= CaptureErrors;
            if (Keyboard.current != null) InputState.Change(Keyboard.current, new KeyboardState());
            InputSystem.settings.backgroundBehavior = _backgroundBehavior;
            Application.runInBackground = _runInBackground;
            if (_camera != null) _camera.enabled = true;
            if (Camera.main != null) Camera.main.transform.rotation = _cameraRotation;
            if (_player != null)
            {
                Teleport(_startPosition);
                _player.transform.rotation = _startRotation;
                _animator.Play("Locomotion", 0, 0f);
            }
            _report.completed = completed;
            _report.passed = completed && _report.checks.All(r => r.passed);
            Directory.CreateDirectory("Tools/ThuyKieu");
            File.WriteAllText("Tools/ThuyKieu/Validation.json", JsonUtility.ToJson(_report, true));
            IsRunning = false;
            Debug.Log("ThuyKieu Play Mode validation: " + (_report.passed ? "PASS" : "FAIL") + "; Tools/ThuyKieu/Validation.json");
        }
    }
}
