using UnityEngine;
using UnityEngine.InputSystem;

namespace ThuyKieu.Core
{
    /// <summary>
    /// Single entry point for raw device input.
    /// Gameplay scripts ask this class instead of touching Keyboard/Mouse directly,
    /// so we can later swap to an InputActionAsset without rewriting gameplay code.
    /// </summary>
    public static class InputReader
    {
        /// <summary>WASD as a normalized-ish vector: x = strafe, y = forward.</summary>
        public static Vector2 Move
        {
            get
            {
                Keyboard keyboard = Keyboard.current;
                if (keyboard == null)
                {
                    return Vector2.zero;
                }

                Vector2 move = Vector2.zero;
                if (keyboard.wKey.isPressed) move.y += 1f;
                if (keyboard.sKey.isPressed) move.y -= 1f;
                if (keyboard.dKey.isPressed) move.x += 1f;
                if (keyboard.aKey.isPressed) move.x -= 1f;

                return Vector2.ClampMagnitude(move, 1f);
            }
        }

        /// <summary>Mouse delta for this frame.</summary>
        public static Vector2 Look
        {
            get
            {
                Mouse mouse = Mouse.current;
                return mouse == null ? Vector2.zero : mouse.delta.ReadValue();
            }
        }

        /// <summary>Left Shift held.</summary>
        public static bool IsRunHeld
        {
            get
            {
                Keyboard keyboard = Keyboard.current;
                return keyboard != null && keyboard.leftShiftKey.isPressed;
            }
        }

        /// <summary>E pressed this frame.</summary>
        public static bool InteractPressedThisFrame
        {
            get
            {
                Keyboard keyboard = Keyboard.current;
                return keyboard != null && keyboard.eKey.wasPressedThisFrame;
            }
        }

        /// <summary>Escape pressed this frame (used to release the cursor in the editor).</summary>
        public static bool CancelPressedThisFrame
        {
            get
            {
                Keyboard keyboard = Keyboard.current;
                return keyboard != null && keyboard.escapeKey.wasPressedThisFrame;
            }
        }
    }
}
