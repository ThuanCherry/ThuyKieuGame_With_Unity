using System;
using System.Collections.Generic;
using ThuyKieu.Core;
using ThuyKieu.Player;
using UnityEngine;

namespace ThuyKieu.Dialogue
{
    /// <summary>Optional presentation cues; unknown Ink tags safely preserve the current shot.</summary>
    public class DialogueTagRouter : MonoBehaviour
    {
        [Serializable] private struct CameraCue { public string Name; public Transform Preset; }
        [SerializeField] private Chapter01Director _director;
        [SerializeField] private ThirdPersonCameraController _camera;
        [SerializeField] private CameraCue[] _cameraCues;
        [SerializeField] private DialogueManager _dialogue;
        [SerializeField] private Transform _kieu, _mother;
        [SerializeField] private Transform _twoShot, _motherShot, _kieuShot;
        [SerializeField] private Transform _ma, _maTwoShot, _maShot, _maKieuShot;
        private bool _maConversation;
        private float _nextMaShotTime;
        private bool _motherConversation;
        private float _establishUntil;
        private Transform _pendingShot;
        public event Action<string> EmotionChanged;
        private readonly HashSet<string> _missing = new HashSet<string>();

        private void OnEnable()
        {
            if (_director != null) _director.CueReceived += Route;
            if (_dialogue == null) _dialogue = GetComponent<DialogueManager>();
            if (_dialogue == null) return;
            _dialogue.DialogueStarted += Begin;
            _dialogue.InkTagsChanged += RouteMotherLine;
            _dialogue.DialogueEnded += End;
        }
        private void OnDisable()
        {
            if (_director != null) _director.CueReceived -= Route;
            if (_dialogue == null) return;
            _dialogue.DialogueStarted -= Begin;
            _dialogue.InkTagsChanged -= RouteMotherLine;
            _dialogue.DialogueEnded -= End;
        }
        private void Begin(DialogueData data)
        {
            _maConversation = _director != null && _director.MaConversation && _camera != null
                && _kieu != null && _ma != null && _maTwoShot != null;
            _motherConversation = _director != null && _director.MotherConversation
                && _camera != null && _kieu != null && _mother != null && _twoShot != null;
            if (_maConversation)
            {
                ComposeMaShots();
                _pendingShot = _maTwoShot;
                _establishUntil = Time.time + .85f;
                _nextMaShotTime = _establishUntil;
                _camera.BeginCinematicDialogue(_maTwoShot);
                return;
            }
            if (!_motherConversation) return;
            ComposeFamilyShots();
            _pendingShot = _twoShot;
            _establishUntil = Time.time + .85f;
            _camera.BeginCinematicDialogue(_twoShot);
        }
        private void ComposeFamilyShots()
        {
            Vector3 kieu = Head(_kieu), mother = Head(_mother);
            Vector3 across = mother - kieu; across.y = 0;
            if (across.sqrMagnitude < .01f) across = -_mother.forward;
            across.Normalize();
            // Keep all three shots on one side of the actors' eyeline.
            Vector3 side = Vector3.Cross(across, Vector3.up);
            Vector3 middle = (kieu + mother) * .5f;
            Compose(_twoShot, middle + side * Mathf.Max(2.8f, Vector3.Distance(kieu, mother) * 1.2f) + Vector3.up * .2f, middle - Vector3.up * .35f);
            Compose(_motherShot, mother - across * 1.30f + side * .80f + Vector3.up * .18f, mother - Vector3.up * .15f);
            Compose(_kieuShot, kieu + across * 1.40f + side * .85f, kieu - Vector3.up * .15f);
        }
        private static Vector3 Head(Transform actor)
        {
            var animator = actor.GetComponentInChildren<Animator>();
            Transform head = animator != null && animator.isHuman ? animator.GetBoneTransform(HumanBodyBones.Head) : null;
            return head != null ? head.position : actor.position + Vector3.up * 1.4f;
        }
        private static void Compose(Transform preset, Vector3 position, Vector3 focus)
        {
            if (preset != null) preset.SetPositionAndRotation(position, Quaternion.LookRotation(focus - position));
        }
        private void RouteMotherLine(string[] tags)
        {
            if (_maConversation) { RouteMaLine(tags); return; }
            if (!_motherConversation) return;
            string speaker = null, cameraTag = null, emotion = null;
            foreach (string raw in tags)
            {
                int colon = raw.IndexOf(':'); if (colon < 0) continue;
                string key = raw.Substring(0, colon).Trim(), value = raw.Substring(colon + 1).Trim();
                if (key == "speaker") speaker = value;
                if (key == "camera") cameraTag = value;
                if (key == "emotion") emotion = value;
            }
            Transform shot = null;
            switch (cameraTag)
            {
                case "WS_MainHall": case "WS_VuongGia": case "Dialogue_TwoShot":
                case "MCU_Kieu_Me": case "MCU_MeKieu_Kieu": shot = _twoShot; break;
                case "MCU_MeKieu": case "Dialogue_MeKieu": shot = _motherShot; break;
                case "MCU_Kieu": case "CU_Kieu": case "Dialogue_Kieu": shot = _kieuShot; break;
            }
            if (shot == null && (emotion == "broken" || emotion == "gentle" || emotion == "soft_worried"
                || emotion == "reassuring" || emotion == "soft_firm" || emotion == "protective" || emotion == "gently_smiling")) shot = _twoShot;
            if (shot == null && speaker == "MeKieu") shot = _motherShot;
            if (shot == null && speaker == "Kieu") shot = _kieuShot;
            // Narration without an explicit cue holds the previous framing.
            if (shot != null) _pendingShot = shot;
        }
        private void LateUpdate()
        {
            if ((_motherConversation || _maConversation) && Time.time >= _establishUntil && _pendingShot != null)
            {
                if (_maConversation && Time.time < _nextMaShotTime) return;
                if (_maConversation && _camera.CurrentDialogueCue != _pendingShot)
                {
                    ComposeMaShots();
                    _nextMaShotTime = Time.time + 1.1f;
                }
                _camera.SetDialogueCue(_pendingShot);
            }
        }
        private void ComposeMaShots()
        {
            Vector3 kieu = Head(_kieu), ma = Head(_ma);
            Vector3 across = ma - kieu; across.y = 0;
            if (across.sqrMagnitude < .01f) across = Vector3.forward;
            across.Normalize();
            Vector3 side = Vector3.Cross(across, Vector3.up);
            Vector3 middle = (kieu + ma) * .5f;
            Compose(_maTwoShot, middle + side * Mathf.Max(2.6f, Vector3.Distance(kieu, ma) * .9f) + Vector3.up * .2f, middle - Vector3.up * .35f);
            if (!VisibleFrom(_maTwoShot, kieu) || !VisibleFrom(_maTwoShot, ma))
            {
                // A side shot can look through the front wall at the doorway. Move along
                // the same side of the eyeline until both faces fit through the opening.
                bool found = false;
                foreach (float depth in new[] { -2.5f, -3.5f, -1.5f, 1.5f, 2.5f })
                {
                    foreach (float width in new[] { 1.3f, 2f, 2.6f })
                    {
                        Compose(_maTwoShot, middle + side * width + across * depth + Vector3.up * .25f, middle - Vector3.up * .35f);
                        if (VisibleFrom(_maTwoShot, kieu) && VisibleFrom(_maTwoShot, ma)) { found = true; break; }
                    }
                    if (found) break;
                }
            }
            // One-shot gestures can raise the head before the neutral talking pose settles.
            // Keep this model's medium shot anchored to its standing face height.
            Vector3 maAnchor = new Vector3(ma.x, _ma.position.y + Mathf.Clamp(ma.y - _ma.position.y, 1.35f, 1.55f), ma.z);
            Compose(_maShot, maAnchor - across * 1.45f + side * .85f + Vector3.up * .1f, maAnchor - Vector3.up * .25f);
            Compose(_maKieuShot, kieu + across * 1.45f + side * .85f, kieu - Vector3.up * .25f);
        }
        private static bool VisibleFrom(Transform shot, Vector3 head)
        {
            if (Physics.CheckSphere(shot.position, .15f, 1 << 8, QueryTriggerInteraction.Ignore)
                || Physics.Linecast(shot.position, head, 1 << 8, QueryTriggerInteraction.Ignore)) return false;
            Vector3 local = Quaternion.Inverse(shot.rotation) * (head - shot.position);
            if (local.z <= 0) return false;
            float halfHeight = local.z * Mathf.Tan(21f * Mathf.Deg2Rad);
            float aspect = Camera.main != null ? Camera.main.aspect : 16f / 9f;
            float x = .5f + local.x / (2 * halfHeight * aspect), y = .5f + local.y / (2 * halfHeight);
            return x > .04f && x < .96f && y > .44f && y < .94f;
        }
        private void RouteMaLine(string[] tags)
        {
            string speaker = null, cameraTag = null;
            foreach (string raw in tags)
            {
                int colon = raw.IndexOf(':'); if (colon < 0) continue;
                string key = raw.Substring(0, colon).Trim(), value = raw.Substring(colon + 1).Trim();
                if (key == "speaker") speaker = value;
                if (key == "camera") cameraTag = value;
            }
            Transform shot = null;
            switch (cameraTag)
            {
                case "Door_Reveal": case "Reveal_MaGiamSinh": case "WS_MainHall":
                case "WS_MainHall_Ma": case "CU_Kieu_Ma": shot = _maTwoShot; break;
                case "CU_Kieu": case "MCU_Kieu": case "CU_Kieu_Eyes": shot = _maKieuShot; break;
                case "MCU_MaGiamSinh": case "CU_MaGiamSinh": shot = _maShot; break;
                case "MCU_MeKieu":
                    ComposeFamilyShots(); shot = _motherShot; break;
                case "MCU_MeKieu_Kieu": case "MCU_Kieu_Me": case "Insert_Hairpin":
                    ComposeFamilyShots(); shot = _twoShot; break;
                default:
                    foreach (CameraCue cue in _cameraCues)
                        if (cue.Name == cameraTag) { shot = cue.Preset; break; }
                    break;
            }
            if (shot == null && speaker == "Kieu") shot = _maKieuShot;
            if (shot == null && speaker == "MaGiamSinh") shot = _maShot;
            if (shot == null && speaker == "MeKieu")
                foreach (CameraCue cue in _cameraCues)
                    if (cue.Name == "MCU_MeKieu") { shot = cue.Preset; break; }
            if (shot != null) _pendingShot = shot;
        }
        private void End()
        {
            if ((_motherConversation || _maConversation) && _camera != null) _camera.SetDialogueCue(null);
            _motherConversation = _maConversation = false; _pendingShot = null;
        }
        private void Route(string key, string value)
        {
            if (key == "emotion") EmotionChanged?.Invoke(value);
            if (_motherConversation || _maConversation || _camera == null) return;
            if (key != "camera") return;
            // The director holds the composed bedroom shot through the opening fade.
            if (value == "Fade_Black" || value == "FadeIn_KieuRoom") return;
            if (value == "Gameplay_Kieu") { _camera.SetDialogueCue(null); return; }
            foreach (CameraCue cue in _cameraCues)
                if (cue.Name == value) { _camera.SetDialogueCue(cue.Preset); return; }
            if (_missing.Add(value)) Debug.LogWarning("Chapter 1 camera cue unavailable: " + value + ". Keeping current framing.", this);
        }
    }
}
