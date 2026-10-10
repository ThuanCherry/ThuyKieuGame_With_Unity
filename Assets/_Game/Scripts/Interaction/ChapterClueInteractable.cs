using ThuyKieu.Core;
using UnityEngine;

namespace ThuyKieu.Interaction
{
    public sealed class ChapterClueInteractable : MonoBehaviour, IInteractable, IInteractableDisplayName, IInteractionAvailability
    {
        [SerializeField] private Chapter01Director _director;
        [SerializeField] private string _knot, _flag, _label;
        public bool IsAvailable => _director != null && _director.CanInspect(_flag);
        public string DisplayName => _label;
        public void Interact() { if (IsAvailable) _director.InspectClue(_knot, _flag); }
    }
}
