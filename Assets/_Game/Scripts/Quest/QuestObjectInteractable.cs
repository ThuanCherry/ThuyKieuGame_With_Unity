using ThuyKieu.Interaction;
using UnityEngine;

namespace ThuyKieu.Quest
{
    /// <summary>
    /// Any world object that satisfies an InteractWithObject objective: a letter, a door,
    /// a shrine. Plugs into the existing interaction system through IInteractable.
    /// </summary>
    public class QuestObjectInteractable : MonoBehaviour, IInteractable, IInteractableDisplayName
    {
        [Tooltip("Must match QuestObjective.targetId on the quest asset.")]
        [SerializeField] private string objectId = "object_id";
        [SerializeField] private string displayName = "Object";

        [Tooltip("Disable the object after a successful interaction (picked up, opened...).")]
        [SerializeField] private bool disableAfterInteract = true;

        [Tooltip("Only report progress while at least one quest needs this object.")]
        [SerializeField] private QuestData requiredQuest;

        public string ObjectId => objectId;
        public string DisplayName => displayName;

        public void Interact()
        {
            if (QuestManager.Instance == null)
            {
                Debug.LogWarning("[QuestObject] No QuestManager in the scene.", this);
                return;
            }

            if (requiredQuest != null && !QuestManager.Instance.IsActive(requiredQuest))
            {
                Debug.Log("[QuestObject] " + displayName + " is not relevant yet.", this);
                return;
            }

            QuestManager.Instance.ReportProgress(ObjectiveType.InteractWithObject, objectId);

            if (disableAfterInteract)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
