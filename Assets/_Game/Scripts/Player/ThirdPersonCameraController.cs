using ThuyKieu.Core;
using UnityEngine;

namespace ThuyKieu.Player
{
    /// <summary>
    /// Simple orbit camera: follows a target transform, mouse rotates around it.
    /// Lives on the camera, not on the player, so movement and camera stay independent.
    /// Can be replaced by Cinemachine later; the player scripts do not depend on it.
    /// </summary>
    public class ThirdPersonCameraController : MonoBehaviour
    {
        [Header("Target")]
        [Tooltip("Usually the CameraTarget child of the Player.")]
        [SerializeField] private Transform target;
        [Tooltip("Used when no target is assigned: finds the object with this tag, then this child.")]
        [SerializeField] private string fallbackTargetTag = "Player";
        [SerializeField] private string fallbackTargetChildName = "CameraTarget";

        [Header("Framing")]
        [SerializeField] private float distance = 4.5f;
        [SerializeField] private Vector3 targetOffset = Vector3.zero;
        [Tooltip("How quickly the camera catches up with the target. 0 = instant.")]
        [SerializeField] private float followSmoothTime = 0.05f;

        [Header("Rotation")]
        [SerializeField] private float mouseSensitivity = 0.15f;
        [SerializeField] private float minPitch = -30f;
        [SerializeField] private float maxPitch = 70f;
        [SerializeField] private bool invertY = false;

        [Header("Cursor")]
        [SerializeField] private bool lockCursorOnStart = true;

        private float yaw;
        private float pitch;
        private Vector3 followVelocity;
        [SerializeField] private LayerMask _collisionMask;
        public bool ControlLocked { get; set; }
        private Transform _dialogueCue;
        public void SetDialogueCue(Transform preset) { _dialogueCue = preset; }

        private void Awake()
        {
            if (target == null)
            {
                target = ResolveFallbackTarget();
            }

            Vector3 euler = transform.eulerAngles;
            yaw = euler.y;
            pitch = Mathf.Clamp(NormalizeAngle(euler.x), minPitch, maxPitch);
        }

        private void Start()
        {
            if (lockCursorOnStart)
            {
                SetCursorLocked(true);
            }
        }

        private void LateUpdate()
        {
            if (ControlLocked && _dialogueCue != null)
            {
                Vector3 shotPosition = _dialogueCue.position;
                if (target != null && _dialogueCue.IsChildOf(target.root))
                    shotPosition = ResolveObstruction(target.position, shotPosition);
                Vector3 nextPosition = Vector3.Lerp(transform.position, shotPosition, 1 - Mathf.Exp(-5 * Time.deltaTime));
                transform.position = ResolveObstruction(transform.position, nextPosition);
                transform.rotation = Quaternion.Slerp(transform.rotation, _dialogueCue.rotation, 1 - Mathf.Exp(-5 * Time.deltaTime));
                return;
            }
            if (target == null)
            {
                return;
            }

            if (!ControlLocked) HandleCursorToggle();

            if (!ControlLocked && Cursor.lockState == CursorLockMode.Locked)
            {
                Vector2 look = InputReader.Look * mouseSensitivity;
                yaw += look.x;
                pitch += invertY ? look.y : -look.y;
                pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            }

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 pivot = target.position + targetOffset;
            Vector3 desiredPosition = pivot - rotation * Vector3.forward * distance;
            if (_collisionMask.value != 0 && Physics.SphereCast(pivot, 0.2f, -(rotation * Vector3.forward),
                    out RaycastHit obstruction, distance, _collisionMask, QueryTriggerInteraction.Ignore))
                desiredPosition = pivot - rotation * Vector3.forward * Mathf.Max(0.3f, obstruction.distance - 0.1f);

            transform.position = followSmoothTime > 0f
                ? Vector3.SmoothDamp(transform.position, desiredPosition, ref followVelocity, followSmoothTime)
                : desiredPosition;

            transform.rotation = rotation;
        }

        private Vector3 ResolveObstruction(Vector3 origin, Vector3 destination)
        {
            Vector3 delta = destination - origin;
            float length = delta.magnitude;
            if (_collisionMask.value != 0 && length > .001f && Physics.SphereCast(origin, .15f,
                    delta / length, out RaycastHit hit, length, _collisionMask, QueryTriggerInteraction.Ignore))
                return origin + delta / length * Mathf.Max(0, hit.distance - .03f);
            return destination;
        }

        /// <summary>Lets other systems (dialogue, menus) release camera control later on.</summary>
        public void SetCursorLocked(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
        }

        private void HandleCursorToggle()
        {
            if (InputReader.CancelPressedThisFrame)
            {
                SetCursorLocked(Cursor.lockState != CursorLockMode.Locked);
            }
        }

        private Transform ResolveFallbackTarget()
        {
            if (string.IsNullOrEmpty(fallbackTargetTag))
            {
                return null;
            }

            GameObject player = GameObject.FindGameObjectWithTag(fallbackTargetTag);
            if (player == null)
            {
                return null;
            }

            Transform child = player.transform.Find(fallbackTargetChildName);
            return child != null ? child : player.transform;
        }

        private static float NormalizeAngle(float angle)
        {
            return angle > 180f ? angle - 360f : angle;
        }
    }
}
