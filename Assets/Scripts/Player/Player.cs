using System.Collections;
using Player.StateMachine;
using Player.StateMachine.States;
using UnityEngine;
using Zenject;

namespace Player
{
    /// <summary>
    /// Core Player controller. Owns the FSM and acts as a shared blackboard for all states.
    ///
    /// Horizontal movement is delegated to MovementComponent.Move() every FixedUpdate.
    /// The three-rule logic (input / air no-op / ground brake) lives in MovementComponent
    /// so no other class needs to reason about it.
    ///
    /// The wall-jump lockout coroutine forces MoveInput = 0 for its duration, which
    /// causes Move() to take the "air no-op" path automatically — no extra flags needed.
    /// </summary>
    [RequireComponent(typeof(MovementComponent))]
    public class Player : MonoBehaviour
    {
        // ──────────────────────────────────────────────────────────────────────────────
        #region Inspector Fields

        [Header("Ground Detection")]
        [Tooltip("Transform placed at the player's feet.")]
        public Transform groundCheck;

        [Tooltip("Radius of the ground-check overlap circle.")]
        public float groundCheckDistance = 0.1f;

        [Tooltip("Layer(s) considered as ground.")]
        public LayerMask groundLayer;

        [Header("Wall Detection")]
        [Tooltip("Transform placed on the left side of the player.")]
        public Transform wallCheckLeft;

        [Tooltip("Transform placed on the right side of the player.")]
        public Transform wallCheckRight;

        [Tooltip("Radius of each wall-check overlap circle.")]
        [SerializeField] private float wallCheckRadius = 0.1f;

        [Tooltip("Layer(s) considered as walls.")]
        [SerializeField] private LayerMask wallLayer;

        [Header("Movement Settings")]
        public float speed = 5f;
        public float jumpForce = 15f;

        [Header("Wall Jump Settings")]
        [SerializeField] private float wallJumpForceX = 10f;
        [SerializeField] private float wallJumpForceY = 15f;
        [SerializeField] private float wallJumpLockTime = 0.1f;

        [Header("Dash Settings")]
        public float dashDuration = 0.125f;
        public float dashSpeed    = 30f;

        #endregion


        // ──────────────────────────────────────────────────────────────────────────────
        #region Public Blackboard

        /// <summary>
        /// Raw horizontal input axis (-1, 0, +1).
        /// Forced to 0 by the wall-jump coroutine during the lockout window.
        /// </summary>
        public float MoveInput { get; private set; }

        /// <summary>Last non-zero horizontal input. Used to determine dash direction.</summary>
        public float LastMoveInput { get; private set; }

        /// <summary>True for the frame the Jump button was pressed.</summary>
        public bool JumpPressed  { get; private set; }

        /// <summary>True for the frame the Jump button was released.</summary>
        public bool JumpReleased { get; private set; }

        /// <summary>True for the frame the Dash button was pressed.</summary>
        public bool DashPressed  { get; private set; }

        /// <summary>Whether the player is touching the ground.</summary>
        public bool IsGrounded { get; private set; }

        /// <summary>Whether the player is touching a wall and not grounded.</summary>
        public bool IsOnWall { get; private set; }

        /// <summary>True when touching a wall on the left side.</summary>
        public bool IsOnLeftWall  { get; private set; }

        /// <summary>True when touching a wall on the right side.</summary>
        public bool IsOnRightWall { get; private set; }

        /// <summary>Whether a dash is currently in progress.</summary>
        public bool IsDashing { get; set; }

        /// <summary>Whether the player can perform a dash.</summary>
        public bool CanDash { get; set; }

        /// <summary>
        /// True while the wall-jump lockout coroutine is running.
        /// JumpState uses this to suppress re-attachment to the same wall.
        /// </summary>
        public bool IsWallJumping { get; private set; }

        /// <summary>Whether the variable jump-cut is active (managed by JumpState).</summary>
        public bool JumpCutting { get; set; }

        #endregion


        // ──────────────────────────────────────────────────────────────────────────────
        #region Components & States

        /// <summary>Low-level physics API. The only class that writes to Rigidbody.</summary>
        public MovementComponent MovementComponent { get; private set; }

        public IdleState IdleState { get; private set; }
        public JumpState JumpState { get; private set; }
        public FallState FallState { get; private set; }
        public DashState DashState { get; private set; }
        public WallState WallState { get; private set; }

        #endregion


        // ──────────────────────────────────────────────────────────────────────────────
        #region Private Fields

        private PlayerStateMachine _fsm;

        [Inject] private InputSystem_Actions _inputActions;

        #endregion


        // ──────────────────────────────────────────────────────────────────────────────
        #region Unity Lifecycle

        private void Awake()
        {
            _fsm              = gameObject.AddComponent<PlayerStateMachine>();
            MovementComponent = GetComponent<MovementComponent>();
        }

        private void Start()
        {
            IdleState = new IdleState(_fsm, this, _inputActions);
            JumpState = new JumpState(_fsm, this, _inputActions);
            FallState = new FallState(_fsm, this, _inputActions);
            DashState = new DashState(_fsm, this, _inputActions);
            WallState = new WallState(_fsm, this, _inputActions);

            _inputActions.Enable();
            _fsm.Initialize(IdleState);
        }

        private void Update()
        {
            RefreshInput();
            RefreshSensors();
            _fsm.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            // While dashing, DashState owns the Rigidbody entirely this tick.
            if (IsDashing)
            {
                _fsm.CurrentState.PhysicsUpdate();
                return;
            }

            // All horizontal movement logic lives in MovementComponent.Move().
            // Passing MoveInput = 0 during wall-jump lockout triggers the air no-op
            // automatically, so the impulse arc carries without any extra flag here.
            MovementComponent.Move(MoveInput, speed, IsGrounded);

            _fsm.CurrentState.PhysicsUpdate();
        }

        private void OnDisable()
        {
            _inputActions?.Disable();
        }

        #endregion


        // ──────────────────────────────────────────────────────────────────────────────
        #region Wall Jump

        /// <summary>
        /// Delegates the wall-jump impulse to MovementComponent and starts the
        /// input-lockout coroutine. Called by WallState when Jump is pressed.
        /// </summary>
        public void WallJump()
        {
            float direction = IsOnLeftWall ? 1f : -1f;

            MovementComponent.WallJump(wallJumpForceX, wallJumpForceY, direction);

            // Restore dash charge so the player can dash out of a wall jump.
            CanDash = true;

            StartCoroutine(WallJumpLockCoroutine());
        }

        /// <summary>
        /// Freezes horizontal input for <see cref="wallJumpLockTime"/> seconds so the
        /// lateral impulse is not cancelled by the player holding a direction.
        /// MoveInput = 0 causes MovementComponent.Move() to take the "air no-op" path,
        /// preserving X velocity without any additional logic.
        /// </summary>
        private IEnumerator WallJumpLockCoroutine()
        {
            IsWallJumping = true;

            float elapsed = 0f;
            while (elapsed < wallJumpLockTime)
            {
                MoveInput  = 0f;
                elapsed   += Time.deltaTime;
                yield return null;
            }

            IsWallJumping = false;
        }

        #endregion


        // ──────────────────────────────────────────────────────────────────────────────
        #region Sensors

        private void RefreshInput()
        {
            JumpPressed  = _inputActions.Player.Jump.WasPressedThisFrame();
            JumpReleased = _inputActions.Player.Jump.WasReleasedThisFrame();
            DashPressed  = _inputActions.Player.Dash.WasPressedThisFrame();

            // Do not read real input while the lockout coroutine is managing MoveInput.
            if (!IsWallJumping)
            {
                MoveInput = _inputActions.Player.Move.ReadValue<Vector2>().x;
                if (MoveInput != 0f)
                    LastMoveInput = MoveInput;
            }
        }

        private void RefreshSensors()
        {
            CheckGrounded();
            CheckWalls();
        }

        private void CheckGrounded()
        {
            IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckDistance, groundLayer);

            if (IsGrounded)
            {
                JumpCutting = false;
                CanDash     = true;
            }
        }

        private void CheckWalls()
        {
            IsOnLeftWall  = Physics2D.OverlapCircle(wallCheckLeft.position,  wallCheckRadius, wallLayer);
            IsOnRightWall = Physics2D.OverlapCircle(wallCheckRight.position, wallCheckRadius, wallLayer);
            IsOnWall      = (IsOnLeftWall || IsOnRightWall) && !IsGrounded;
        }

        #endregion


        // ──────────────────────────────────────────────────────────────────────────────
        #region Gizmos

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            if (groundCheck    != null) Gizmos.DrawWireSphere(groundCheck.position,    groundCheckDistance);
            if (wallCheckLeft  != null) Gizmos.DrawWireSphere(wallCheckLeft.position,  wallCheckRadius);
            if (wallCheckRight != null) Gizmos.DrawWireSphere(wallCheckRight.position, wallCheckRadius);
        }
#endif

        #endregion
    }
}