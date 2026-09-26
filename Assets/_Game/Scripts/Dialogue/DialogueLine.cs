using System;
using UnityEngine;

namespace ThuyKieu.Dialogue
{
    /// <summary>One spoken line. Speaker can be overridden so a node can host a short exchange.</summary>
    [Serializable]
    public class DialogueLine
    {
        [Tooltip("Leave empty to use the speaker name of the DialogueData.")]
        [SerializeField] private string speakerOverride = string.Empty;
        [SerializeField, TextArea(2, 5)] private string text = string.Empty;

        public string SpeakerOverride => speakerOverride;
        public string Text => text;
    }
}
