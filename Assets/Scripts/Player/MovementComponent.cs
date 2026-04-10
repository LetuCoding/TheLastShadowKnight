using UnityEngine;

namespace Player
{
    /// <summary>
    /// Low-level physics API. All velocity and gravity writes go through here —
    /// no other class touches Rigidbody directly.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementComponent : MonoBehaviour
    {
        // ──────────────────────────────────────────────────────────────────────────────
        #region Public References

        /// <summary>The Rigidbody2D attached to the player. Read-only from outside.</summary>
        public Rigidbody2D Rigidbody { get; private set; }

        #endregion


        // ──────────────────────────────────────────────────────────────────────────────
        #region Unity Lifecycle

        private void Awake()
        {
            // Assigned in Awake so Rigidbody is ready before any Start() runs,
            // preventing a NullReferenceException when the FSM initialises.
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        #endregion


        // ──────────────────────────────────────────────────────────────────────────────
        #region Movement API

        /// <summary>
        /// Applies horizontal movement based on <paramref name="input"/> and <paramref name="speed"/>.
        /// Preserves current vertical velocity.
        /// <para>
        /// Callers are responsible for passing 0 when movement should be suppressed
        /// (e.g. wall-jump lockout), which causes the in-air no-op path to trigger
        /// naturally without any extra flags here.
        /// </para>
        /// </summary>
        /// <param name="input">Raw horizontal axis value (-1, 0, +1).</param>
        /// <param name="speed">Horizontal speed in units/second.</param>
        /// <param name="isGrounded">Used to decide whether to brake or preserve momentum.</param>
        public void Move(float input, float speed, bool isGrounded)
        {
            if (input != 0f)
            {
                // Active input: full speed in that direction.
                Rigidbody.linearVelocity = new Vector2(input * speed, Rigidbody.linearVelocity.y);
            }
            else if (isGrounded)
            {
                // On the ground with no input: brake immediately.
                Rigidbody.linearVelocity = new Vector2(0f, Rigidbody.linearVelocity.y);
            }
            // Air + no input: intentional no-op.
            // Current X is preserved so wall-jump arcs carry naturally.
        }

        /// <summary>
        /// Sets vertical velocity to <paramref name="jumpForce"/>, preserving X.
        /// </summary>
        public void Jump(float jumpForce)
        {
            Rigidbody.linearVelocity = new Vector2(Rigidbody.linearVelocity.x, jumpForce);
        }

        /// <summary>
        /// Applies a wall-jump impulse away from the wall.
        /// </summary>
        /// <param name="forceX">Horizontal launch force.</param>
        /// <param name="forceY">Vertical launch force.</param>
        /// <param name="direction">+1 (right, jumping off left wall) or -1 (left, jumping off right wall).</param>
        public void WallJump(float forceX, float forceY, float direction)
        {
            // Zero first for a consistent launch angle every time.
            Rigidbody.linearVelocity = Vector2.zero;
            Rigidbody.linearVelocity = new Vector2(direction * forceX, forceY);
        }

        /// <summary>
        /// Launches the player horizontally at dash speed, zeroing vertical velocity.
        /// </summary>
        /// <param name="direction">+1 (right) or -1 (left).</param>
        /// <param name="speed">Total dash speed.</param>
        public void Dash(float direction, float speed)
        {
            Rigidbody.linearVelocity = new Vector2(direction * speed, 0f);
        }

        /// <summary>Overrides only the vertical component of the current velocity.</summary>
        public void SetVelocityY(float y)
        {
            Rigidbody.linearVelocity = new Vector2(Rigidbody.linearVelocity.x, y);
        }

        /// <summary>Overrides only the horizontal component of the current velocity.</summary>
        public void SetVelocityX(float x)
        {
            Rigidbody.linearVelocity = new Vector2(x, Rigidbody.linearVelocity.y);
        }

        /// <summary>Sets the Rigidbody2D gravity scale.</summary>
        public void SetGravity(float gravityScale)
        {
            Rigidbody.gravityScale = gravityScale;
        }

        /// <summary>
        /// Returns the facing direction from horizontal velocity.
        /// Falls back to the transform's local X scale when velocity is zero.
        /// </summary>
        public float GetFacingDirection()
        {
            return Rigidbody.linearVelocity.x != 0f
                ? Mathf.Sign(Rigidbody.linearVelocity.x)
                : Mathf.Sign(transform.localScale.x);
        }

        #endregion
    }
}