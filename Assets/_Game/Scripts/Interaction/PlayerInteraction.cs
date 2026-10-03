using ThuyKieu.Core;

using UnityEngine;

namespace ThuyKieu.Interaction
{
    /// <summary>
    /// Finds the best IInteractable in front of the player and triggers it.
    /// This class knows nothing about what an interactable does (dialogue, pickup, door...).
    /// </summary>
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Detection")]
        [SerializeField] private float interactionDistance = 2.5f;
        [Tooltip("Radius of the sphere cast, so the player does not need pixel perfect aim.")]
        [SerializeField] private float interactionRadius = 0.4f;
        [Tooltip("Which layers can be interacted with. Default: everything.")]
        [SerializeField] private LayerMask interactionLayers = ~0;
        [SerializeField] private QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Collide;

        [Header("References")]
        [Tooltip("Origin and direction of the detection cast. Leave empty to use this transform.")]
        [SerializeField] private Transform detectionOrigin;
        [Tooltip("Vertical offset from the origin, so the cast leaves the feet of the character.")]
        [SerializeField] private float detectionHeight = 1f;

        /// <summary>The interactable currently targeted, or null. UI can poll this for a prompt.</summary>
        public IInteractable CurrentTarget { get; private set; }
        public bool ControlLocked { get; set; }

        private void Awake()
        {
            if (detectionOrigin == null)
            {
                detectionOrigin = transform;
            }
        }

        private void Update()
        {
            if (ControlLocked) { CurrentTarget = null; return; }
            CurrentTarget = FindInteractable();

            if (CurrentTarget != null && InputReader.InteractPressedThisFrame)
            {
                CurrentTarget.Interact();
            }
        }

        private IInteractable FindInteractable()
        {
            // Opt-in range detection also works when standing beside an NPC.
            IInteractable nearest = null;
            float bestDistance = interactionDistance;
            foreach (Collider collider in Physics.OverlapSphere(transform.position, interactionDistance, interactionLayers, triggerInteraction))
            {
                var availability = collider.GetComponentInParent<IInteractionAvailability>();
                var candidate = collider.GetComponentInParent<IInteractable>();
                if (availability == null || !availability.IsAvailable || candidate == null) continue;
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < bestDistance) { nearest = candidate; bestDistance = distance; }
            }
            if (nearest != null) return nearest;
            Vector3 origin = detectionOrigin.position + Vector3.up * detectionHeight;
            Vector3 direction = detectionOrigin.forward;

            if (!Physics.SphereCast(origin, interactionRadius, direction, out RaycastHit hit,
                    interactionDistance, interactionLayers, triggerInteraction))
            {
                return null;
            }

            // GetComponentInParent so a collider on a child of an NPC still resolves to the NPC.
            var available = hit.collider.GetComponentInParent<IInteractionAvailability>();
            if (available != null && !available.IsAvailable) return null;
            return hit.collider.GetComponentInParent<IInteractable>();
        }

        private void OnDrawGizmosSelected()
        {
            Transform origin = detectionOrigin != null ? detectionOrigin : transform;
            Vector3 start = origin.position + Vector3.up * detectionHeight;
            Vector3 end = start + origin.forward * interactionDistance;

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(start, end);
            Gizmos.DrawWireSphere(end, interactionRadius);
        }
    }
}
