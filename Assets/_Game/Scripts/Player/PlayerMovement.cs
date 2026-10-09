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

        [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField] private float _animationSmoothTime = 0.08f;
        [Tooltip("Use 0 / 0.5 / 1 for idle / walk / run. Leave off for controllers using metres per second.")]
        [SerializeField] private bool _normalizeAnimatorSpeed;
        private static readonly int SpeedParameter = Animator.StringToHash("Speed");

        private CharacterController controller;
        private float verticalVelocity;

        /// <summary>Current planar speed in units/second. Useful for the animation team.</summary>
        public float CurrentSpeed { get; private set; }

        /// <summary>True while the run input is held and the character is actually moving.</summary>
        public bool IsRunning { get; private set; }
        public bool ControlLocked { get; set; }
        /// <summary>Used by authored seated poses that must not be pulled down by the CharacterController.</summary>
        public bool PositionLocked { get; set; }

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            if (_animator == null) _animator = GetComponentInChildren<Animator>();
            if (_animator != null) _animator.applyRootMotion = false;

            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        private void Update()
        {
            if (PositionLocked)
            {
                CurrentSpeed = 0f;
                IsRunning = false;
                if (_animator != null) _animator.SetFloat(SpeedParameter, 0f);
                return;
            }
            Vector3 moveDirection = GetCameraRelativeDirection(ControlLocked ? Vector2.zero : InputReader.Move);

            bool wantsToRun = InputReader.IsRunHeld && moveDirection.sqrMagnitude > 0f;
            float speed = wantsToRun ? runSpeed : walkSpeed;

            RotateTowards(moveDirection);
            ApplyGravity();

            Vector3 velocity = moveDirection * speed;
            velocity.y = verticalVelocity;
            Vector3 previousPosition = transform.position;
            controller.Move(velocity * Time.deltaTime);

            Vector3 displacement = transform.position - previousPosition;
            displacement.y = 0f;
            CurrentSpeed = Time.deltaTime > 0f ? displacement.magnitude / Time.deltaTime : 0f;
            IsRunning = wantsToRun && CurrentSpeed > walkSpeed + 0.01f;
            if (_animator != null && _animator.runtimeAnimatorController != null)
            {
                float animationSpeed = CurrentSpeed;
                if (_normalizeAnimatorSpeed)
                    animationSpeed = CurrentSpeed <= walkSpeed
                        ? Mathf.InverseLerp(0f, walkSpeed, CurrentSpeed) * 0.5f
                        : 0.5f + Mathf.InverseLerp(walkSpeed, runSpeed, CurrentSpeed) * 0.5f;
                _animator.SetFloat(SpeedParameter, animationSpeed, _animationSmoothTime, Time.deltaTime);
            }
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
