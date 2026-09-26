using UnityEngine;

namespace ThuyKieu.Interaction
{
    /// <summary>
    /// Placeholder interactable used to validate the interaction pipeline.
    /// Delete once real NPC / item / door interactables exist.
    /// </summary>
    public class TestInteractable : MonoBehaviour, IInteractable, IInteractableDisplayName
    {
        [SerializeField] private string displayName = "Test Cube";

        public string DisplayName => displayName;

        public void Interact()
        {
            Debug.Log("Interaction successful", this);
        }
    }
}
