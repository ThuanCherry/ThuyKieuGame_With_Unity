using ThuyKieu.Core;
using UnityEngine;

namespace ThuyKieu.Player
{
    /// <summary>
    /// Camera-relative character movement. This class owns movement only:
    /// no camera logic, no interaction, no animation state machine.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Speed")]
        [SerializeField] private float walkSpeed = 2.5f;
        [SerializeField] private float runSpeed = 5.5f;
        [Tooltip("How fast the character turns towards the movement direction (deg/s).")]
        [SerializeField] private float rotationSpeed = 720f;

        [Header("Gravity")]
        [SerializeField] private float gravity = -20f;
        [Tooltip("Downward force kept while grounded so the controller stays glued to slopes.")]
        [SerializeField] private float groundedStickForce = -2f;

        [Header("References")]
        [Tooltip("Movement is relative to this transform. Leave empty to fall back to Camera.main.")]
        [SerializeField] private Transform cameraTransform;

        private CharacterController controller;
        private float verticalVelocity;

        /// <summary>Current planar speed in units/second. Useful for the animation team.</summary>
        public float CurrentSpeed { get; private set; }

        /// <summary>True while the run input is held and the character is actually moving.</summary>
        public bool IsRunning { get; private set; }

        private void Awake()
        {
            controller = GetComponent<CharacterController>();

            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        private void Update()
        {
            Vector3 moveDirection = GetCameraRelativeDirection(InputReader.Move);

            IsRunning = InputReader.IsRunHeld && moveDirection.sqrMagnitude > 0f;
            float speed = IsRunning ? runSpeed : walkSpeed;

            RotateTowards(moveDirection);
            ApplyGravity();

            Vector3 velocity = moveDirection * speed;
            velocity.y = verticalVelocity;
            controller.Move(velocity * Time.deltaTime);

            CurrentSpeed = new Vector3(velocity.x, 0f, velocity.z).magnitude;
        }

        /// <summary>Projects the 2D input onto the horizontal plane of the camera.</summary>
        private Vector3 GetCameraRelativeDirection(Vector2 input)
        {
            if (input.sqrMagnitude < 0.0001f)
            {
                return Vector3.zero;
            }

            Vector3 forward = Vector3.forward;
            Vector3 right = Vector3.right;

            if (cameraTransform != null)
            {
                forward = cameraTransform.forward;
                right = cameraTransform.right;
            }

            forward.y = 0f;
            right.y = 0f;

            return (forward.normalized * input.y + right.normalized * input.x).normalized;
        }

        private void RotateTowards(Vector3 direction)
        {
            if (direction.sqrMagnitude < 0.0001f)
            {
                return;
            }

            Quaternion target = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed * Time.deltaTime);
        }

        private void ApplyGravity()
        {
            if (controller.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = groundedStickForce;
                return;
            }

            verticalVelocity += gravity * Time.deltaTime;
        }
    }
}
