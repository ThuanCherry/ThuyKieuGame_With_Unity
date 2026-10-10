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
        private bool _snapToCue;
        [Header("Cinematic dialogue")]
        [SerializeField, Range(.4f, .8f)] private float _dialogueBlendSeconds = .65f;
        [SerializeField, Range(35, 50)] private float _dialogueFieldOfView = 42;
        [SerializeField, Min(0)] private float _rotationSmoothTime = .10f;
        private Camera _lens;
        private float _gameplayFieldOfView, _blendStarted, _blendFromFov;
        private Vector3 _blendFromPosition;
        private Quaternion _blendFromRotation;
        private float _smoothedYaw, _smoothedPitch, _yawVelocity, _pitchVelocity;
        private bool _cinematic, _returning;
        public Transform CurrentDialogueCue => _dialogueCue;
        public bool IsBlending => (_cinematic || _returning) && Time.time - _blendStarted < _dialogueBlendSeconds;
        public void BeginCinematicDialogue(Transform preset)
        {
            _cinematic = true;
            SetDialogueCue(preset);
        }

        private void StartBlend()
        {
            _blendStarted = Time.time;
            _blendFromPosition = transform.position;
            _blendFromRotation = transform.rotation;
            _blendFromFov = _lens != null ? _lens.fieldOfView : 60;
            followVelocity = Vector3.zero;
        }
        public void SetDialogueCue(Transform preset)
        {
            if (preset == _dialogueCue) return;
            if (_cinematic)
            {
                StartBlend();
                if (preset == null) { _cinematic = false; _returning = true; }
            }
            _dialogueCue = preset;
            // A cut between rooms is preferable to interpolating into a partition and getting stuck.
            _snapToCue = preset != null && Physics.Linecast(transform.position, preset.position, _collisionMask, QueryTriggerInteraction.Ignore);
        }

        private void Awake()
        {
            if (target == null)
            {
                target = ResolveFallbackTarget();
            }

            Vector3 euler = transform.eulerAngles;
            yaw = euler.y;
            pitch = Mathf.Clamp(NormalizeAngle(euler.x), minPitch, maxPitch);
            _smoothedYaw = yaw; _smoothedPitch = pitch;
            _lens = GetComponent<Camera>();
            _gameplayFieldOfView = _lens != null ? _lens.fieldOfView : 60;
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
                if (_cinematic)
                {
                    BlendTo(shotPosition, _dialogueCue.rotation, _dialogueFieldOfView);
                    return;
                }
                if (target != null && _dialogueCue.IsChildOf(target.root))
                    shotPosition = ResolveObstruction(target.position, shotPosition);
                if (_snapToCue && !Physics.CheckSphere(shotPosition, .15f, _collisionMask, QueryTriggerInteraction.Ignore))
                {
                    transform.SetPositionAndRotation(shotPosition, _dialogueCue.rotation);
                    _snapToCue = false;
                    return;
                }
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

            if (!ControlLocked && !_returning && Cursor.lockState == CursorLockMode.Locked)
            {
                Vector2 look = InputReader.Look * mouseSensitivity;
                yaw += look.x;
                pitch += invertY ? look.y : -look.y;
                pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            }

            _smoothedYaw = Mathf.SmoothDampAngle(_smoothedYaw, yaw, ref _yawVelocity, _rotationSmoothTime);
            _smoothedPitch = Mathf.SmoothDampAngle(_smoothedPitch, pitch, ref _pitchVelocity, _rotationSmoothTime);
            Quaternion rotation = Quaternion.Euler(_smoothedPitch, _smoothedYaw, 0f);
            Vector3 pivot = target.position + targetOffset;
            Vector3 desiredPosition = pivot - rotation * Vector3.forward * distance;
            if (_collisionMask.value != 0 && Physics.SphereCast(pivot, 0.2f, -(rotation * Vector3.forward),
                    out RaycastHit obstruction, distance, _collisionMask, QueryTriggerInteraction.Ignore))
                desiredPosition = pivot - rotation * Vector3.forward * Mathf.Max(0.3f, obstruction.distance - 0.1f);

            if (_returning)
            {
                BlendTo(desiredPosition, rotation, _gameplayFieldOfView);
                if (!IsBlending) _returning = false;
                return;
            }

            Vector3 smoothedPosition = followSmoothTime > 0f
                ? Vector3.SmoothDamp(transform.position, desiredPosition, ref followVelocity, followSmoothTime)
                : desiredPosition;
            // Smoothing must not put the lens back behind the obstruction just resolved above.
            transform.position = ResolveObstruction(pivot, smoothedPosition);

            transform.rotation = rotation;
        }

        private void BlendTo(Vector3 position, Quaternion rotation, float fieldOfView)
        {
            float t = Mathf.SmoothStep(0, 1, Mathf.Clamp01((Time.time - _blendStarted) / _dialogueBlendSeconds));
            Vector3 next = Vector3.Lerp(_blendFromPosition, position, t);
            transform.position = ResolveObstruction(transform.position, next);
            transform.rotation = Quaternion.Slerp(_blendFromRotation, rotation, t);
            if (_lens != null) _lens.fieldOfView = Mathf.Lerp(_blendFromFov, fieldOfView, t);
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
