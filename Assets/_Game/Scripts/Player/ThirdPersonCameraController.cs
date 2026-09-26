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
            if (target == null)
            {
                return;
            }

            HandleCursorToggle();

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Vector2 look = InputReader.Look * mouseSensitivity;
                yaw += look.x;
                pitch += invertY ? look.y : -look.y;
                pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            }

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 pivot = target.position + targetOffset;
            Vector3 desiredPosition = pivot - rotation * Vector3.forward * distance;

            transform.position = followSmoothTime > 0f
                ? Vector3.SmoothDamp(transform.position, desiredPosition, ref followVelocity, followSmoothTime)
                : desiredPosition;

            transform.rotation = rotation;
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
