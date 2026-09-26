using System;
using UnityEngine;

namespace ThuyKieu.Dialogue
{
    /// <summary>
    /// Runs a conversation: walks the lines of a node, hands choices to the view,
    /// follows branches and reports completion. It owns no UI: any view can listen
    /// to the events below, so Developer 6 can replace the view without touching this.
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

        private Action onConversationComplete;

        /// <summary>Current node, or null when idle.</summary>
        public DialogueData CurrentNode { get; private set; }

        /// <summary>Index of the line being shown inside CurrentNode.</summary>
        public int CurrentLineIndex { get; private set; }

        public bool IsDialogueActive => CurrentNode != null;

        /// <summary>True when the last line of the node is on screen and choices should show.</summary>
        public bool IsAtChoicePoint => IsDialogueActive
                                       && CurrentNode.HasChoices
                                       && CurrentLineIndex >= CurrentNode.Lines.Count - 1;

        /// <summary>Raised when a conversation opens. Views show themselves here.</summary>
        public event Action<DialogueData> DialogueStarted;

        /// <summary>Speaker name, line text. Raised for every line, including after a branch.</summary>
        public event Action<string, string> LineChanged;

        /// <summary>Raised when choices must be displayed (empty array = hide choices).</summary>
        public event Action<DialogueChoice[]> ChoicesChanged;

        /// <summary>Raised when the conversation closes, after the completion callback.</summary>
        public event Action DialogueEnded;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        /// <summary>
        /// Opens a conversation. onComplete fires once, when the whole conversation
        /// (including branches) is over. This is the hook NPCs use to start quests.
        /// </summary>
        public bool StartDialogue(DialogueData node, Action onComplete = null)
        {
            if (node == null)
            {
                Debug.LogWarning("[Dialogue] StartDialogue called with no DialogueData.", this);
                return false;
            }

            if (IsDialogueActive)
            {
                return false;
            }

            onConversationComplete = onComplete;
            CurrentNode = node;
            CurrentLineIndex = 0;

            if (DialogueStarted != null)
            {
                DialogueStarted(node);
            }

            PublishCurrentLine();
            return true;
        }

        /// <summary>Continue button / continue key. Advances one line, or closes at the end.</summary>
        public void Advance()
        {
            if (!IsDialogueActive)
            {
                return;
            }

            if (IsAtChoicePoint)
            {
                // Waiting for the player to pick a branch.
                return;
            }

            if (CurrentLineIndex < CurrentNode.Lines.Count - 1)
            {
                CurrentLineIndex++;
                PublishCurrentLine();
                return;
            }

            EndDialogue();
        }

        /// <summary>Follows a branch. Index refers to CurrentNode.Choices.</summary>
        public void SelectChoice(int choiceIndex)
        {
            if (!IsDialogueActive || !CurrentNode.HasChoices)
            {
                return;
            }

            if (choiceIndex < 0 || choiceIndex >= CurrentNode.Choices.Count)
            {
                return;
            }

            DialogueChoice choice = CurrentNode.Choices[choiceIndex];
            DialogueData next = choice.NextNode;

            if (next == null)
            {
                EndDialogue();
                return;
            }

            CurrentNode = next;
            CurrentLineIndex = 0;
            PublishCurrentLine();
        }

        /// <summary>Closes the conversation and fires the completion callback exactly once.</summary>
        public void EndDialogue()
        {
            if (!IsDialogueActive)
            {
                return;
            }

            CurrentNode = null;
            CurrentLineIndex = 0;

            if (ChoicesChanged != null)
            {
                ChoicesChanged(new DialogueChoice[0]);
            }

            Action callback = onConversationComplete;
            onConversationComplete = null;

            if (callback != null)
            {
                callback();
            }

            if (DialogueEnded != null)
            {
                DialogueEnded();
            }
        }

        private void PublishCurrentLine()
        {
            string speaker = CurrentNode.GetSpeakerFor(CurrentLineIndex);
            string text = CurrentNode.Lines.Count > 0 ? CurrentNode.Lines[CurrentLineIndex].Text : string.Empty;

            if (LineChanged != null)
            {
                LineChanged(speaker, text);
            }

            if (ChoicesChanged == null)
            {
                return;
            }

            if (IsAtChoicePoint)
            {
                var array = new DialogueChoice[CurrentNode.Choices.Count];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = CurrentNode.Choices[i];
                }

                ChoicesChanged(array);
            }
            else
            {
                ChoicesChanged(new DialogueChoice[0]);
            }
        }
    }
}
