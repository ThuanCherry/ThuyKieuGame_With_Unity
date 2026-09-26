using System.Collections.Generic;
using UnityEngine;

namespace ThuyKieu.Dialogue
{
    /// <summary>
    /// One dialogue node: a speaker, a run of lines, and optionally a set of player choices.
    /// Branching is done by pointing choices at other DialogueData assets, so writers can
    /// build a conversation graph without touching code.
    /// Deliberately knows nothing about UI, quests or the player.
    /// </summary>
    [CreateAssetMenu(fileName = "Dialogue_", menuName = "ThuyKieu/Dialogue", order = 0)]
    public class DialogueData : ScriptableObject
    {
        [SerializeField] private string speakerName = "NPC";
        [SerializeField] private List<DialogueLine> lines = new List<DialogueLine>();

        [Tooltip("Shown after the last line. Empty = the conversation just ends.")]
        [SerializeField] private List<DialogueChoice> choices = new List<DialogueChoice>();

        public string SpeakerName => speakerName;
        public IReadOnlyList<DialogueLine> Lines => lines;
        public IReadOnlyList<DialogueChoice> Choices => choices;
        public bool HasChoices => choices != null && choices.Count > 0;

        public string GetSpeakerFor(int lineIndex)
        {
            if (lineIndex < 0 || lineIndex >= lines.Count)
            {
                return speakerName;
            }

            string over = lines[lineIndex].SpeakerOverride;
            return string.IsNullOrEmpty(over) ? speakerName : over;
        }
    }
}
