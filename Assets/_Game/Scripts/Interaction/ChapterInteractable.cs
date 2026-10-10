using ThuyKieu.Core;
using UnityEngine;

namespace ThuyKieu.Interaction
{
    public class ChapterInteractable : MonoBehaviour, IInteractable, IInteractableDisplayName, IInteractionAvailability
    {
        [SerializeField] private Chapter01Director _director;
        [SerializeField] private Chapter01Director.Stage _stage;
        [SerializeField] private string _label;
        public bool IsAvailable => _director != null && _director.CanInteract(_stage);
        public string DisplayName => _label;
        public void Interact() { if (IsAvailable) _director.Interact(_stage, transform); }
    }
}
