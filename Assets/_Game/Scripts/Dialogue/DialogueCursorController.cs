using ThuyKieu.Player;
using UnityEngine;

namespace ThuyKieu.Dialogue
{
    /// <summary>
    /// Frees the mouse cursor while a conversation is open so the player can click choices,
    /// and re-locks it afterwards. Kept as its own component so the dialogue core stays
    /// independent of the camera: delete this object and dialogue still works via keyboard.
    /// </summary>
    public class DialogueCursorController : MonoBehaviour
    {
        [Tooltip("Leave empty to find the third person camera controller automatically.")]
        [SerializeField] private ThirdPersonCameraController cameraController;

        private DialogueManager manager;

        private void Start()
        {
            if (cameraController == null)
            {
                cameraController = FindAnyObjectByType<ThirdPersonCameraController>();
            }

            manager = DialogueManager.Instance;
            if (manager == null)
            {
                return;
            }

            manager.DialogueStarted += HandleStarted;
            manager.DialogueEnded += HandleEnded;
        }

        private void OnDestroy()
        {
            if (manager == null)
            {
                return;
            }

            manager.DialogueStarted -= HandleStarted;
            manager.DialogueEnded -= HandleEnded;
        }

        private void HandleStarted(DialogueData data)
        {
            if (cameraController != null)
            {
                cameraController.SetCursorLocked(false);
            }
        }

        private void HandleEnded()
        {
            if (cameraController != null)
            {
                cameraController.SetCursorLocked(true);
            }
        }
    }
}
