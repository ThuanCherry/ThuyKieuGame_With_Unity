using ThuyKieu.Interaction;
using ThuyKieu.Quest;
using UnityEngine;

namespace ThuyKieu.Dialogue
{
    /// <summary>
    /// The bridge between the interaction system and the dialogue system.
    ///
    ///   Player -> PlayerInteraction -> IInteractable.Interact() -> NPCInteractable -> DialogueManager
    ///
    /// Nothing in PlayerMovement, PlayerInteraction or the camera had to change for this to work.
    /// </summary>
    public class NPCInteractable : MonoBehaviour, IInteractable, IInteractableDisplayName
    {
        [Header("Identity")]
        [Tooltip("Must match QuestObjective.targetId for TalkToNPC objectives.")]
        [SerializeField] private string npcId = "npc_id";
        [SerializeField] private string displayName = "NPC";

        [Header("Dialogue")]
        [Tooltip("Conversation played the first time, and whenever no other branch applies.")]
        [SerializeField] private DialogueData defaultDialogue;

        [Tooltip("Played while questToStart is Active (a reminder line).")]
        [SerializeField] private DialogueData questActiveDialogue;

        [Tooltip("Unlocked once questToStart is Completed. This is the quest -> dialogue link.")]
        [SerializeField] private DialogueData questCompletedDialogue;

        [Header("Quest")]
        [Tooltip("Started when the default conversation finishes. This is the dialogue -> quest link.")]
        [SerializeField] private QuestData questToStart;

        public string NpcId => npcId;
        public string DisplayName => displayName;

        public void Interact()
        {
            DialogueManager dialogue = DialogueManager.Instance;
            if (dialogue == null)
            {
                Debug.LogWarning("[NPC] No DialogueManager in the scene.", this);
                return;
            }

            // The player can hold E; ignore re-entry while the panel is up.
            if (dialogue.IsDialogueActive)
            {
                return;
            }

            DialogueData node = SelectDialogue();
            if (node == null)
            {
                Debug.LogWarning("[NPC] " + displayName + " has no dialogue assigned.", this);
                return;
            }

            dialogue.StartDialogue(node, OnDialogueComplete);
        }

        /// <summary>Picks the conversation that matches the current quest state.</summary>
        private DialogueData SelectDialogue()
        {
            QuestManager quests = QuestManager.Instance;

            if (quests != null && questToStart != null)
            {
                QuestStatus status = quests.GetStatus(questToStart);

                if (status == QuestStatus.Completed && questCompletedDialogue != null)
                {
                    return questCompletedDialogue;
                }

                if (status == QuestStatus.Active && questActiveDialogue != null)
                {
                    return questActiveDialogue;
                }
            }

            return defaultDialogue;
        }

        private void OnDialogueComplete()
        {
            QuestManager quests = QuestManager.Instance;
            if (quests == null)
            {
                return;
            }

            // Talking is itself an objective type.
            quests.ReportProgress(ObjectiveType.TalkToNPC, npcId);

            // Dialogue completed -> start quest.
            if (questToStart != null)
            {
                quests.StartQuest(questToStart);
            }
        }
    }
}
