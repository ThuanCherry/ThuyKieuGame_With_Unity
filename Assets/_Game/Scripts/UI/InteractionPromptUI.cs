using ThuyKieu.Dialogue;
using ThuyKieu.Interaction;
using UnityEngine;
using UnityEngine.UI;

namespace ThuyKieu.UI
{
    /// <summary>
    /// Shows a small "[E] ..." hint while the player is looking at something interactable.
    /// Reads the data PlayerInteraction already exposes; the interaction system stays UI free.
    /// </summary>
    public class InteractionPromptUI : MonoBehaviour
    {
        [SerializeField] private GameObject promptRoot;
        [SerializeField] private Text promptText;

        [Tooltip("Leave empty to find the player interaction component automatically.")]
        [SerializeField] private PlayerInteraction playerInteraction;

        [SerializeField] private string keyLabel = "E";

        private void Start()
        {
            if (playerInteraction == null)
            {
                playerInteraction = FindAnyObjectByType<PlayerInteraction>();
            }

            Show(false, string.Empty);
        }

        private void Update()
        {
            if (playerInteraction == null)
            {
                return;
            }

            // A conversation owns the screen; do not stack hints on top of it.
            DialogueManager dialogue = DialogueManager.Instance;
            if (dialogue != null && dialogue.IsDialogueActive)
            {
                Show(false, string.Empty);
                return;
            }

            IInteractable target = playerInteraction.CurrentTarget;
            if (target == null)
            {
                Show(false, string.Empty);
                return;
            }

            IInteractableDisplayName named = target as IInteractableDisplayName;
            string label = named != null ? named.DisplayName : string.Empty;
            Show(true, "[" + keyLabel + "] " + label);
        }

        private void Show(bool visible, string text)
        {
            if (promptRoot != null && promptRoot.activeSelf != visible)
            {
                promptRoot.SetActive(visible);
            }

            if (visible && promptText != null && promptText.text != text)
            {
                promptText.text = text;
            }
        }
    }
}
