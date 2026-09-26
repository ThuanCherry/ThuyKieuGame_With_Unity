using ThuyKieu.Core;
using UnityEngine;

namespace ThuyKieu.Interaction
{
    /// <summary>
    /// A doorway that moves the player to another scene. Implements the existing
    /// IInteractable contract, so PlayerInteraction drives it with no changes:
    /// the collider is a trigger, and PlayerInteraction already casts against triggers.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class SceneDoorInteractable : MonoBehaviour, IInteractable, IInteractableDisplayName
    {
        [Header("Destination")]
        [Tooltip("Scene name as listed in Build Settings.")]
        [SerializeField] private string targetScene = "KieuHome_Interior";

        [Tooltip("Spawn id the PlayerSpawner of the target scene looks up.")]
        [SerializeField] private string targetSpawnId = "Interior";

        [Header("Prompt")]
        [Tooltip("Shown by InteractionPromptUI as: [E] <label>")]
        [SerializeField] private string promptLabel = "Vao nha";

        public string DisplayName => promptLabel;

        public void Interact()
        {
            if (string.IsNullOrEmpty(targetScene))
            {
                Debug.LogWarning("[SceneDoor] No target scene set.", this);
                return;
            }

            SceneTransition.Travel(targetScene, targetSpawnId);
        }
    }
}
