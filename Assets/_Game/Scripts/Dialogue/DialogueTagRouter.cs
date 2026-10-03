using System;
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
        public event Action<string> EmotionChanged;

        private void OnEnable() { if (_director != null) _director.CueReceived += Route; }
        private void OnDisable() { if (_director != null) _director.CueReceived -= Route; }
        private void Route(string key, string value)
        {
            if (key == "emotion") EmotionChanged?.Invoke(value);
            if (key != "camera") return;
            foreach (CameraCue cue in _cameraCues)
                if (cue.Name == value) { _camera.SetDialogueCue(cue.Preset); return; }
        }
    }
}
