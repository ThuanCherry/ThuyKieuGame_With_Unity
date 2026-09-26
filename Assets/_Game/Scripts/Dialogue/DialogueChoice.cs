using System;
using UnityEngine;

namespace ThuyKieu.Dialogue
{
    /// <summary>
    /// A player reply. Leaving nextNode empty ends the conversation after this choice,
    /// which still counts as completing the dialogue.
    /// </summary>
    [Serializable]
    public class DialogueChoice
    {
        [SerializeField, TextArea(1, 3)] private string choiceText = "...";

        [Tooltip("Dialogue node to jump to. Empty = end the conversation.")]
        [SerializeField] private DialogueData nextNode;

        [Tooltip("Optional id a listener can react to (analytics, flags, later branching).")]
        [SerializeField] private string choiceId = string.Empty;

        public string ChoiceText => choiceText;
        public DialogueData NextNode => nextNode;
        public string ChoiceId => choiceId;
    }
}
